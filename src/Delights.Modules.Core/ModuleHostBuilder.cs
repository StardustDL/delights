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
        protected Dictionary<Type, IModule> Descriptors { get; } = new Dictionary<Type, IModule>();

        public IReadOnlyList<IModule> Modules => Descriptors.Values.ToArray();

        public IModuleHostBuilder AddModule(Type type, IModule module)
        {
            if (Descriptors.ContainsKey(type))
            {
                throw new Exception($"Module with type {type.Name} has been added.");
            }
            Descriptors.Add(type, module);
            module.Setup(this);
            return this;
        }

        public IModule? GetModule(Type type)
        {
            if (Descriptors.TryGetValue(type, out var value))
                return value;
            else
                return null;
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
        }
    }
}