namespace AwwareCmds.Modules
{
    public interface IModuleInfo
    {
        ModuleKind Kind { get; }
        string ModuleName { get; }
        string Version { get; }
        string Author { get; }
        void ModuleInitialize(Executer exec);
        void ModuleDeinitialize(Executer exec);
    }
}
