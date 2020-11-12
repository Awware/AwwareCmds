using AwwareCmds.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwwareCmds
{
    public abstract class AbstractCommand
    {
        public abstract string Name { get; }
        public abstract string InputCommand { get; }
        public abstract string InputCommandAbbr { get; }
        public abstract void CommandInitialization();
        public abstract Task<CommandResult> CommandExecute(object[] attributes, string[] subcmds);
        public virtual string GetDescription()
        {
            return "*empty*";
        }
        public virtual string GetSyntax()
        {
            return "*empty*";
        }
    }
}
