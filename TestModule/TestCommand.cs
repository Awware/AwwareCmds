using AwwareCmds;
using AwwareCmds.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestCommand : AbstractCommand
    {
        public override string Name => "Test Command";

        public override string InputCommand => "test";

        public override string InputCommandAbbr => "t";

        public override async Task<CommandResult> CommandExecute(object[] attributes, string[] subcmds)
        {
            TaskCompletionSource<CommandResult> res = new TaskCompletionSource<CommandResult>();
            res.SetResult(CommandResult.Default);
            CommandService.Interactor.Debug("Test!!!");
            await Task.Delay(25000);
            CommandService.Interactor.Debug("After sleep!!!");
            return res.Task.Result;
        }

        public override void CommandInitialization()
        {

        }
    }
}
