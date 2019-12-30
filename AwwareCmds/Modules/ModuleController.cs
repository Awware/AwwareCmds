using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AwwareCmds.Modules
{
    public class ModuleController
    {
        private readonly Executer EXEC;
        public List<Module> Modules;
        public ModuleController(Executer exec)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ASM_RESOLVE);
            EXEC = exec;
            Modules = new List<Module>();
        }

        public Module GenerateModule(Assembly ModuleAssembly) => new Module(GetModuleInfo(ModuleAssembly), ModuleAssembly, Loader.LoadCommands(ModuleAssembly));
        public void AttachModule(Module mod)
        {
            if (ModuleAttached(mod))
                throw new Exception($"Module already attached! ({mod.ModuleInfo.ModuleName})");
            mod.ModuleInfo.ModuleInitialize(EXEC); //Module init
            InitializeModuleCommands(mod);         //Module commands init
            Modules.Add(mod);                      //Adding to modules heap
            EXEC.CommandsHeap.AddRange(mod.ModuleCmds); //Adding module commands to global commands heap
        }
        public bool ModuleAttached(Module mod) => Modules.Where(x => x.ModuleInfo.ModuleName == mod.ModuleInfo.ModuleName).Count() > 0;
        private void InitializeModuleCommands(Module mod)
        {
            foreach (var cmd in mod.ModuleCmds)
                cmd.C_Init(EXEC);
        }
        public void DetachModule(Module mod)
        {
            if (mod == null)
                throw new Exception("Module not found!");
            if (mod.ModuleInfo.Kind == ModuleKind.System)
                return;
            mod.ModuleInfo.ModuleDeinitialize(EXEC);
            DetachCommandsHeap(mod);        //Remove from global cmds heap
            Modules.Remove(mod);            //Remove from global modules heap
        }
        public void DetachAllModules()
        {
            foreach (var mod in Modules.ToArray())
                DetachModule(mod);
        }
        private void DetachCommandsHeap(Module mod)
        {
            foreach (var cmd in mod.ModuleCmds)
                EXEC.CommandsHeap.Remove(cmd);
        }
        public void DetachModule(string name) => DetachModule(GetModuleByName(name));
        public void DisableModule(string name)
        {
            Module mod = GetModuleByName(name);
            if (mod == null)
                throw new Exception("Module not found!");
            if (mod.ModuleInfo.Kind == ModuleKind.System)
                return;
            mod.DisableEnableModule();
            DetachCommandsHeap(mod);
        }
        public void EnableModule(string name)
        {
            Module mod = GetModuleByName(name);
            if(mod == null)
                throw new Exception("Module not found!");
            if (!mod.Disabled)
                return;
            mod.DisableEnableModule();
            EXEC.CommandsHeap.AddRange(mod.ModuleCmds);
        }
        public void ReloadModules()
        {
            foreach (var mod in Modules)
                ReloadModule(mod);
        }
        public void ReloadModule(string mName)
        {
            ReloadModule(GetModuleByName(mName));
        }
        public void ReloadModule(Module module)
        {
            module.ModuleInfo.ModuleDeinitialize(EXEC);
            DetachModule(module);
            EXEC.AttachModule(File.ReadAllBytes(module.ModuleAssembly.Location));
        }
        public Module GetModuleByName(string name)
        {
            return Modules.Where(x => x.ModuleInfo.ModuleName == name).FirstOrDefault();
        }
        public Module GetModuleByRelativeName(string relName) => Modules.Where(x => x.ModuleInfo.ModuleName.Contains(relName)).FirstOrDefault();
        
        public IModuleInfo GetModuleInfo(System.Reflection.Assembly ModAsm)
        {
            foreach (var type in ModAsm.GetTypes())
                if (typeof(IModuleInfo).IsAssignableFrom(type) && type != typeof(IModuleInfo))
                    return Activator.CreateInstance(type) as IModuleInfo;
            throw new Exception($"IModule not found! | {ModAsm.FullName}");
        }

        private Assembly ASM_RESOLVE(object sender, ResolveEventArgs args)
        {
            Assembly MyAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";
            objExecutingAssemblies = args.RequestingAssembly;
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    strTempAssmbPath = $"{EXEC.LastModulesFolder}\\{args.Name.Substring(0, args.Name.IndexOf(","))}.dll";
                    break;
                }
            }
            MyAssembly = Assembly.Load(File.ReadAllBytes(strTempAssmbPath));
            return MyAssembly;
        }
    }
}
