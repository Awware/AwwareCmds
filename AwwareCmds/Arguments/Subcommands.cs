using System;
using System.Collections.Generic;

namespace AwwareCmds.Arguments
{
    public class Subcommands : ArgsTemplate
    {
        public List<string> SubCommands;
        public Subcommands(ArgsController controller) : base(controller)
        {
            CONTROLLER = controller;
        }
        public string this[int index]
        {
            get
            {
                return (!HasSubcommands() && (SubCommands.Count - 1) < index) ? "" : SubCommands[index];
            }
        }

        public bool IsSubcmd(int index, string subcmd)
        {
            return ((SubCommands.Count - 1) < index && !HasSubcommands()) ? false : SubCommands[index] == subcmd;
        }

        public override void Handle()
        {
            SubCommands = GetSubcommands();   
        }

        public bool HasSubcommands()
        {
            return SubCommands.Count > 0;
        }

        public List<string> GetSubcommands(int plusindex = 0)
        {
            try
            {
                string[] sC = CONTROLLER.ROWArguments.Split(' ');
                List<string> tSC = new List<string>();
                for (int i = 1 + plusindex; i < sC.Length; i++)
                {
                    if (sC[i].Contains("\"") || System.Text.RegularExpressions.Regex.IsMatch(sC[i], @"^\d+"))
                        break;
                    tSC.Add(sC[i]);
                }
                return tSC;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }
    }
}
