using System;
using System.Collections.Generic;

namespace AwwareCmds
{
    public static class Loader
    {
        public static List<AbstractCommand> LoadCommands(System.Reflection.Assembly asm)
        {
            List<AbstractCommand> Cmds = new List<AbstractCommand>();
            foreach (var type in asm.GetTypes())
                if (typeof(AbstractCommand).IsAssignableFrom(type) && type != typeof(AbstractCommand))
                    Cmds.Add(Activator.CreateInstance(type) as AbstractCommand);
            Validation(Cmds);
            return Cmds;
        }
        //Refactor it
        private static void Validation(List<AbstractCommand> cmds)
        {
            foreach (var cmd in cmds)
            {
                foreach (var cmd2 in cmds)
                {
                    if (cmd == cmd2)
                        continue;
                    if (cmd.Name == cmd2.Name)
                        throw new Exception($"Identical command names! {cmd.Name} - {cmd2.Name} | {cmd.InputCommand}");
                    else if (cmd.InputCommand == cmd2.InputCommand)
                        throw new Exception($"Identical commands! {cmd.InputCommand} - {cmd2.InputCommand} | {cmd.Name}");
                }
            }
        }
    }
}
