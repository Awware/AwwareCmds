using System.Collections.Generic;

namespace AwwareCmds
{
    public interface ICMD
    {
        string Name { get; }
        string Cmd { get; }
        string CmdAbbr { get; }
        string Desc { get; }
        string Syntax { get; }
        List<SubInfo> Subcommands { get; }
        void C_Init(Executer exe);
        void C_Execute(Executer exe, AwwareCmds.Arguments.ArgsController args);
    }
}
