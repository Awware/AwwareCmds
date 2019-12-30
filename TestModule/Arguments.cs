using AwwareCmds;
using AwwareCmds.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class Arguments : ICMD
    {
        //Cmd(CmdAbbr) without '/' 
        public string Name => "Arguments";
        public string Cmd => "args";
        public string CmdAbbr => "a";
        public string Syntax => "";
        public string Desc => "";
        public List<SubInfo> Subcommands => new List<SubInfo>()
        {

        };

        public void C_Execute(Executer exe, ArgsController args)
        {
            if (args.SubCmds.IsSubcmd(0, "str"))
                AEvents.OutputAction(args.StrArgs[0], AEvents.OutTypes.Warning);
            else if (args.SubCmds.IsSubcmd(0, "numb"))
                AEvents.OutputAction($"{args.NumbArgs[0]}", AEvents.OutTypes.Warning);
            else if (args.SubCmds.IsSubcmd(0, "all"))
                AEvents.OutputAction($"{args.NumbArgs[0]} | {args.StrArgs[0]}", AEvents.OutTypes.Warning);
        }

        public void C_Init(Executer exe)
        {
            //AEvents.OutputAction("Argum inited!", AEvents.OutTypes.Info);
        }
    }
}
