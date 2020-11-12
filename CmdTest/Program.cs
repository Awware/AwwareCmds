using AwwareCmds;
using AwwareCmds.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CmdTest
{
    class Program
    {
        class ConsoleInteractor : Interactor
        {
            public static bool Ended { get; set; } = true;
            public void After(CommandResult result)
            {
                Ended = true;
                Console.WriteLine("AFTER!");
            }

            public void Before()
            {
                Ended = false;
                Console.WriteLine("BEFORE");
            }

            public void Clear()
            {

            }

            public void Debug(string msg)
            {
                Console.WriteLine($"DEBUG: {msg}");
            }

            public void Error(string msg)
            {
                Console.WriteLine($"ERROR: {msg}");
            }

            public void Info(string msg)
            {
                Console.WriteLine($"INFO: {msg}");
            }

            public void Sleep(CancellationTokenSource cancelation)
            {
                Console.WriteLine("SLEEP!");
                //cancelation.Cancel();
            }

            public void Success(string msg)
            {
                Console.WriteLine($"SUCCESS: {msg}");
            }

            public void Warning(string msg)
            {
                Console.WriteLine($"WARNING: {msg}");
            }
        }
        public static CommandService exec;
        static void Main(string[] args)
        {
            exec = new CommandService(new ConsoleInteractor());
            exec.AttachModulesFromFolder("Modules");
            while (true) {
                if (!ConsoleInteractor.Ended)
                    continue;
                exec.CommandHandler(Console.ReadLine());
            }
        }
    }
}
