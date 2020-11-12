using System.Collections.Generic;
using System.Reflection;

namespace AwwareCmds.Modules
{
    public class Module
    {
        public Module(IModuleInfo modInfo, System.Reflection.Assembly asm, List<AbstractCommand> cmds)
        {
            ModuleInfo = modInfo;
            ModuleAssembly = asm;
            ModuleCmds = cmds;
        }

        public IModuleInfo ModuleInfo { get; set; }
        public Assembly ModuleAssembly { get; set; }
        public List<AbstractCommand> ModuleCmds { get; set; }
        public bool Disabled { get; set; } = false;
        public void DisableEnableModule() => Disabled = !Disabled;
    }
}
