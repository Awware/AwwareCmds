using System;
using System.Collections.Generic;

namespace AwwareCmds
{
    public static class Loader
    {
        public static List<ICMD> LoadCommands(System.Reflection.Assembly asm)
        {
            List<ICMD> Cmds = new List<ICMD>();
            foreach (var type in asm.GetTypes())
                if (typeof(ICMD).IsAssignableFrom(type) && type != typeof(ICMD))
                    Cmds.Add(Activator.CreateInstance(type) as ICMD);
            Validation(Cmds);
            return Cmds;
        }
        //Refactor it
        private static void Validation(List<ICMD> cmds)
        {
            foreach (var cmd in cmds)
            {
                foreach (var cmd2 in cmds)
                {
                    if (cmd == cmd2)
                        continue;
                    if (cmd.Name == cmd2.Name)
                        throw new Exception($"Identical command names! {cmd.Name} - {cmd2.Name} | {cmd.Cmd}");
                    else if (cmd.Cmd == cmd2.Cmd)
                        throw new Exception($"Identical commands! {cmd.Cmd} - {cmd2.Cmd} | {cmd.Name}");
                }
            }
        }
    }
}
