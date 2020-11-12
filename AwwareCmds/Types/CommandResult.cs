using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwwareCmds.Types
{
    public class CommandResult
    {
        public readonly static CommandResult Default = new CommandResult();
        public readonly static CommandResult Return = new CommandResult("Return");
        public CommandResult(params object[] args)
        {
            Args = args;
        }
        public CommandResult()
        {
            Args = new object[] { "End" };
        }
        public bool WithError { get; internal set; } = false;
        public object[] Args { get; }
    }
}
