using AwwareCmds;
using AwwareCmds.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestModule
{
    public class Sleep : ICMD
    {
        //Cmd(CmdAbbr) without '/' 
        public string Name => "Sleep";
        public string Cmd => "sleep";
        public string CmdAbbr => "sp";
        public string Syntax => "";
        public string Desc => "";
        public List<SubInfo> Subcommands => new List<SubInfo>()
        {

        };

        public void C_Execute(Executer exe, ArgsController args)
        {
            AEvents.OutputAction("Before Sleep!", AEvents.OutTypes.Info);
            Thread.Sleep(20000);
            AEvents.OutputAction("After Sleep!", AEvents.OutTypes.Info);
        }

        public void C_Init(Executer exe)
        {
            
        }
    }
}
