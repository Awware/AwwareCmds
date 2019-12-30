using AwwareCmds;
using AwwareCmds.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestModule
{
    public class Help : ICMD
    {
        //Cmd(CmdAbbr) without '/' 
        public string Name => "Help";
        public string Cmd => "help";
        public string CmdAbbr => "?";
        public string Syntax => "";
        public string Desc => "";
        public List<SubInfo> Subcommands => new List<SubInfo>()
        {

        };

        public void C_Execute(Executer exe, ArgsController args)
        {
            AEvents.OutputAction("Commands:", AEvents.OutTypes.Info);
            AEvents.OutputAction("...", AEvents.OutTypes.Warning);
        }

        public void C_Init(Executer exe)
        {
            AEvents.OutputAction("Help inited!", AEvents.OutTypes.Info);
        }
    }
}
