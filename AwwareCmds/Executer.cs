using AwwareCmds.Arguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AwwareCmds
{
    public class Executer
    {
        public Executer(bool gc, string modulesFolder, int Timeout = 15000)
        {
            COMMANDTIMEOUT = Timeout;
            ModulesFolder = modulesFolder;
            ArgsControl = new ArgsController();
            CommandsHeap = new List<ICMD>();
            MODController = new Modules.ModuleController(this);
            GarbageCollect = gc;
        }
        public Thread MainThread;
        public List<ICMD> CommandsHeap;
        public bool GarbageCollect = false;
        private System.Diagnostics.Stopwatch myStopwatch = null;

        public ArgsController ArgsControl;
        public Modules.ModuleController MODController;
        public string ModulesFolder;
        public int COMMANDTIMEOUT { get; set; } = 15000;
        public void AttachModule(System.Reflection.Assembly asm) => MODController.AttachModule(MODController.GenerateModule(asm));
        public void AttachModule(byte[] rawAsm) => MODController.AttachModule(MODController.GenerateModule(Assembly.Load(rawAsm)));
        public void AttachModulesFromFolder()
        {
            foreach (var asm in System.IO.Directory.GetFiles(ModulesFolder, "*.module"))
                MODController.AttachModule(MODController.GenerateModule(Assembly.Load(File.ReadAllBytes(asm))));
        }
        public void CommandHandler(string cmd)
        {
            try
            {
                if (cmd.StartsWith("/"))
                {
                    ICMD command = GetCommand(cmd.Split(' ')[0].Remove(0, 1));
                    if (command != null)
                    {
                        myStopwatch = new System.Diagnostics.Stopwatch();

                        myStopwatch.Start();

                        ArgsControl.CommandDist(cmd);

                        Task execute = new Task(() =>
                        {
                            command.C_Execute(this, ArgsControl);
                        });

                        AEvents.InvokeAction("before");

                        Parallel.Invoke(execute.Start, () =>
                        {
                            Task.Factory.StartNew(() =>
                            {
                                while (execute.Status == TaskStatus.Running)
                                {
                                    if (myStopwatch.ElapsedMilliseconds > COMMANDTIMEOUT)
                                    {
                                        AEvents.InvokeAction("sleep", execute);
                                        break;
                                    }
                                }
                                myStopwatch.Stop();
                            });
                        });
                        execute.ContinueWith((task) =>
                        {
                            AEvents.InvokeAction("ended");

                            if (GarbageCollect)
                                GC.Collect();
                        });
                    }
                    else
                        AEvents.OutputAction($"Command '{cmd}' not found!", AEvents.OutTypes.Error);
                }
                else
                    AEvents.OutputAction($"Invalid command!", AEvents.OutTypes.Error);
            }
            catch (Exception ex)
            {
                AEvents.OutputAction($"{ex.Message}\n{ex.StackTrace}", AEvents.OutTypes.Error);
            }
        }
        public ICMD GetCommand(string cmd) => CommandsHeap.Where(a => a.Cmd == cmd || a.CmdAbbr == cmd).FirstOrDefault();
    }
}
