using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Delights.Modules
{
    public static class ModuleExtensions
    {
        public static ModuleCollection AddModules(this IServiceCollection services)
        {
            ModuleCollection modules = new ModuleCollection(services);
            services.AddSingleton(modules);
            return modules;
        }
    }

    public class ModuleCollection
    {
        public ModuleCollection(IServiceCollection services) => Services = services;

        public bool EnabledUI { get; set; } = true;

        public bool EnabledService { get; set; } = true;

        IServiceCollection Services { get; }

        public IList<Module> Modules { get; } = new List<Module>();

        public ModuleCollection EnableUI()
        {
            EnabledUI = true;
            return this;
        }

        public ModuleCollection DisableUI()
        {
            EnabledUI = false;
            return this;
        }

        public ModuleCollection EnableService()
        {
            EnabledService = true;
            return this;
        }

        public ModuleCollection DisableService()
        {
            EnabledService = false;
            return this;
        }

        public ModuleCollection AddModule<T>()
            where T : Module, new() => AddModule(new T());

        public ModuleCollection AddModule<T>(T module)
            where T : Module
        {
            Modules.Add(module);
            Services.TryAddSingleton(module);
            if (EnabledUI)
            {
                module.RegisterUI(Services);
            }
            if (EnabledService)
            {
                module.RegisterService(Services);
            }
            return this;
        }
    }
}
