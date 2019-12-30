namespace AwwareCmds.Arguments
{
    //Шаблон для новых аргов
    public abstract class ArgsTemplate
    {
        public ArgsController CONTROLLER;
        public ArgsTemplate(ArgsController controller)
        {
            CONTROLLER = controller;
        }
        public abstract void Handle();
    }
}
