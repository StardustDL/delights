using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delights.Modules
{
    public static class ModuleHostBuilder
    {
        public static IModuleHostBuilder CreateDefaultBuilder()
        {
            return new DefaultModuleHostBuilder();
        }
    }

    public class DefaultModuleHostBuilder : IModuleHostBuilder
    {
        protected List<(Type, IModule)> Descriptors { get; } = new List<(Type, IModule)>();

        public IReadOnlyList<IModule> Modules => Descriptors.Select(x => x.Item2).ToArray();

        public IModuleHostBuilder AddModule<T>(T module)
            where T : class, IModule
        {
            Descriptors.Add((typeof(T), module));
            return this;
        }

        public void Build(IServiceCollection services)
        {
            var modules = Modules.ToArray();
            services.AddSingleton<IModuleHost>(sp => new ModuleHost(sp, modules));
            foreach (var (type, module) in Descriptors)
            {
                services.AddSingleton(type, module);
                module.RegisterService(services);
            }
            foreach (var module in modules)
            {
                module.Setup(this, services);
            }
        }
    }
}