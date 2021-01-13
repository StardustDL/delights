using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules
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

        protected List<(ModuleHostBuilderMiddlewareType, Action<IModuleHostBuilder, IServiceCollection>)> Middlewares { get; } = new List<(ModuleHostBuilderMiddlewareType, Action<IModuleHostBuilder, IServiceCollection>)>();

        public virtual IReadOnlyList<IModule> Modules => Descriptors.Values.ToArray();

        public virtual IModuleHostBuilder AddModule(Type type, IModule module)
        {
            if (Descriptors.ContainsKey(type))
            {
                throw new Exception($"Module with type {type.Name} has been added.");
            }
            Descriptors.Add(type, module);
            module.Setup(this);
            return this;
        }

        public virtual IModule? GetModule(Type type)
        {
            if (Descriptors.TryGetValue(type, out var value))
                return value;
            else
                return null;
        }

        protected virtual void PreBuild(IServiceCollection services)
        {
            foreach (var middleware in
                from x in Middlewares where x.Item1 is ModuleHostBuilderMiddlewareType.Pre select x.Item2)
            {
                middleware(this, services);
            }
        }

        protected virtual void PostBuild(IServiceCollection services)
        {
            foreach (var middleware in
                from x in Middlewares where x.Item1 is ModuleHostBuilderMiddlewareType.Post select x.Item2)
            {
                middleware(this, services);
            }
        }

        public virtual void Build(IServiceCollection services)
        {
            PreBuild(services);

            var modules = Modules.ToArray();
            services.AddSingleton<IModuleHost>(sp => new ModuleHost(sp, modules));
            foreach (var (type, module) in Descriptors)
            {
                services.AddSingleton(type, module);
                module.RegisterService(services);
            }

            PostBuild(services);
        }

        public IModuleHostBuilder UseMiddleware(ModuleHostBuilderMiddlewareType type, Action<IModuleHostBuilder, IServiceCollection> middleware)
        {
            Middlewares.Add((type, middleware));
            return this;
        }
    }
}