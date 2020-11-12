using AwwareCmds.CommandAnalyser;
using AwwareCmds.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AwwareCmds
{
    public class CommandService
    {
        public static CommandService Singleton { private set; get; }
        private static Interactor _interactor;
        public static Interactor Interactor
        {
            get => _interactor ?? new NullInteractor();
            set => _interactor = value;
        }
        public CommandService(Interactor interact = null)
        {
            Singleton = this;
            Interactor = interact;

            CommandsHeap = new List<AbstractCommand>();
            MODController = new Modules.ModuleController(this);
        }
        public List<AbstractCommand> CommandsHeap;
        public bool GarbageCollect = false;
        public string LastModulesFolder = "";
        private System.Diagnostics.Stopwatch myStopwatch = null;
        public Modules.ModuleController MODController;
        public int Timeout { get; set; } = 15000;
        public void AttachModule(System.Reflection.Assembly asm) => MODController.AttachModule(MODController.GenerateModule(asm));
        public void AttachModule(byte[] rawAsm) => MODController.AttachModule(MODController.GenerateModule(Assembly.Load(rawAsm)));
        public void AttachModulesFromFolder(string folder)
        {
            LastModulesFolder = folder;
            foreach (var asm in System.IO.Directory.GetFiles(folder, "*.module"))
                MODController.AttachModule(MODController.GenerateModule(Assembly.Load(File.ReadAllBytes(asm))));
        }
        public void CommandHandler(string cmd)
        {
            var tokenSource = new CancellationTokenSource();
            var cancel = tokenSource.Token;
            if (RawCommand.TryParse(cmd, out RawCommand rCommand))
            {
                AbstractCommand command = GetCommand(rCommand.Command);
                if (command != null)
                {
                    myStopwatch = new System.Diagnostics.Stopwatch();

                    myStopwatch.Start();

                    Interactor.Before();

                    Task<CommandResult> execution = new Task<CommandResult>(() => command.CommandExecute(rCommand.Attributes.ToArray(), rCommand.Subcommands.ToArray()).GetAwaiter().GetResult(), cancel);

                    execution.Start();

                    execution.ContinueWith((a) => Interactor.After(a.Result));

                    Task.Factory.StartNew(() =>
                    {
                        while (execution.Status != TaskStatus.Faulted || execution.Status != TaskStatus.Canceled || execution.Status != TaskStatus.RanToCompletion)
                        {
                            if (myStopwatch.ElapsedMilliseconds > Timeout)
                            {
                                Interactor.Sleep(tokenSource);
                                myStopwatch.Stop();
                                break;
                            }
                        }
                    });
                }
                else
                    Interactor.Error($"Command '{cmd}' not found!");
            }
            else
                Interactor.Error($"Invalid command!");
        }
        public AbstractCommand GetCommand(string cmd) => CommandsHeap.Where(a => a.InputCommand == cmd || a.InputCommandAbbr == cmd).FirstOrDefault();
    }
}
