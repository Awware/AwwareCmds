namespace AwwareCmds.Arguments
{
    //Контроллер аргументов
    //Основной центр контроля аргов
    public partial class ArgsController
    {
        public string ROWArguments;
        public Subcommands SubCmds; //Отвечает за суб-кмд. /cmd subcmd subcmd
        public NumberArgs NumbArgs; //Отвечает за числа  - %d%
        public StringArgs StrArgs;  //Отвечает за строки - "str"
        public ArgsController()
        {
            SubCmds = new Subcommands(this);
            NumbArgs = new NumberArgs(this);
            StrArgs = new StringArgs(this);
        }
        public void CommandDist(string row)
        {
            ROWArguments = row.Trim();

            SubCmds.Handle();
            StrArgs.Handle();
            NumbArgs.Handle();
        }
    }
}
