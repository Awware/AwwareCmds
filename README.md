# AwwareCmds
.NET Framework library to help implement command line with your commands
# Mini-Manual
<b>With modules</b>
```cs
var executer = new CommandService()
executer.AttachModulesFromFolder("UR FOLDER");
```
<b>If your commands are in this assembly</b>
```cs
var executer = new CommandService()
executer.AttachModule(Assembly.GetExecutingAssembly());
```

<i>For more - see CmdTest and TestModule</i>

<b>How to add command?</b>
```cs
    public class TestCommand : AbstractCommand
    {
        public override string Name => "Test Command";

        public override string InputCommand => "test";

        public override string InputCommandAbbr => "t";

        public override Task<CommandResult> CommandExecute(object[] attributes, string[] subcmds)
        {
            TaskCompletionSource<CommandResult> res = new TaskCompletionSource<CommandResult>();
            res.SetResult(CommandResult.Default);
            return res.Task;
        }

        public override void CommandInitialization()
        {

        }
    }
```

<b>How to use subcommands?</b>
```cs
        public override Task<CommandResult> CommandExecute(object[] attributes, string[] subcmds)
        {
            TaskCompletionSource<CommandResult> res = new TaskCompletionSource<CommandResult>();
            res.SetResult(CommandResult.Default);
            //Instead 'args.SubCmds[0]' you can use args.IsSubcmd(INDEX, "SUBCMD");
            if(subcmds[0] == "test") // -> /CMD test
            {
              CommandService.Interactor.Info("It's 'test' subcommand");
              if(subcmds[1] == "test2") // -> /CMD test test2
              {
                CommandService.Interactor.Info("It's 'test2' subcommand");
                //etc.
              }
            }
            else if(subcmds[1] == "test3") // -> /CMD test3
            {
              //Code
            }
            return res.Task;
        }
```
<b>How to use String arguments?</b>
```cs
        public override Task<CommandResult> CommandExecute(object[] attributes, string[] subcmds)
        {
          TaskCompletionSource<CommandResult> res = new TaskCompletionSource<CommandResult>();
          res.SetResult(CommandResult.Default);
          if(subcmds[0] == "str") // -> /CMD str "My string"
          {
            CommandService.Interactor.Info($"Your string is '{attributes[0]}'");
          }
          return res.Task;
        }
```

<b>How to use NumberArguments?</b>
```cs
        public override Task<CommandResult> CommandExecute(object[] attributes, string[] subcmds)
        {
          TaskCompletionSource<CommandResult> res = new TaskCompletionSource<CommandResult>();
          res.SetResult(CommandResult.Default);
          if(subcmds[0] == "numb") // -> /CMD numb 0.25 25
          {
            CommandService.Interactor.Info($"Your first number is '{(double)attributes[0]}', your second numb is '{(int)attributes[1]}'");
          }
          return res.Task;
        }
```
