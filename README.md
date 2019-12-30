# AwwareCmds
.NET Framework library to help implement command line with your commands
# How it use?
<b>With modules</b>
```cs
var executer = new Executer(GC_BOOL);
executer.AttachModulesFromFolder("UR FOLDER");
```
<b>If your commands are in this assembly</b>
```cs
var executer = new Executer(GC_BOOL)
executer.AttachModule(Assembly.GetExecutingAssembly());
```

<b>Events example</b>
```cs
AEvents.OutputDebug += Action<string>((msg) =>  Console.WriteLine(msg));
```
<i>For more - see CmdTest and TestModule</i>

<b>How use output?</b>
```cs
AEvents.OutputAction("MSG", AEvents.OutTypes.Info); //Types : Info, Debug, Error, Success, Warning
```

<b>How to add command?</b>
```cs
    public class TestCommand : ICMD
    {
        //Cmd(CmdAbbr) without '/' 
        public string Name => "Test Command";
        public string Cmd => "test";
        public string CmdAbbr => "t";
        public string Syntax => "";
        public string Desc => "";
        public List<SubInfo> Subcommands => new List<SubInfo>()
        {
        
        };

        public void C_Execute(Executer exe, ArgsController args)
        {
            AEvents.OutputAction("A test out!", AEvents.OutTypes.Info);
        }

        public void C_Init(Executer exe)
        {
            AEvents.OutputAction("Test Command 'inited'!", AEvents.OutTypes.Info);
        }
    }
```

<b>How to use Subcommands?</b>
```cs
        public void C_Execute(Executer exe, ArgsController args)
        {
            //Instead 'args.SubCmds[0]' you can use args.IsSubcmd(INDEX, "SUBCMD");
            if(args.SubCmds[0] == "test") // -> /CMD test
            {
              AEvents.OutputAction("It's test subcommand", AEvents.OutTypes.Info);
              if(args.SubCmds[1] == "test2") // -> /CMD test test2
              {
                AEvents.OutputAction("It's test2 subcommand", AEvents.OutTypes.Info);
                //etc.
              }
            }
            else if(args.IsSubcmd(0, "test3")) // -> /CMD test3
            {
              //Code
            }
        }
```
<b>How to use StringArguments?</b>
```cs
        public void C_Execute(Executer exe, ArgsController args)
        {
          if(args.IsSubcmd(0, "str")) // -> /CMD str "My string"
          {
            AEvents.OutputAction($"Your string is '{args.StrArgs[0]}'", AEvents.OutTypes.Info);
          }
        }
```

<b>How to use NumberArguments?</b>
```cs
        public void C_Execute(Executer exe, ArgsController args)
        {
          if(args.IsSubcmd(0, "numb")) // -> /CMD numb 0.25 25
          {
            AEvents.OutputAction($"Your first numb is '{args.NumbArgs[0]}', your second numb is '{args.NumbArgs[1]}'", AEvents.OutTypes.Info);
          }
        }
```
