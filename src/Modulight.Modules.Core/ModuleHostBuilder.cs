using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modulight.Modules
{
    /// <summary>
    /// Default methods for module host builders.
    /// </summary>
    public static class ModuleHostBuilder
    {
        /// <summary>
        /// Create a defualt module host builder.
        /// </summary>
        /// <returns></returns>
        public static IModuleHostBuilder CreateDefaultBuilder()
        {
            return new DefaultModuleHostBuilder();
        }
    }

    /// <summary>
    /// Default implement for <see cref="IModuleHostBuilder"/>.
    /// </summary>
    public class DefaultModuleHostBuilder : IModuleHostBuilder
    {
        /// <summary>
        /// Module descriptors registered modules.
        /// </summary>
        protected Dictionary<Type, IModule> Descriptors { get; } = new Dictionary<Type, IModule>();

        /// <summary>
        /// Registered building middlewares.
        /// </summary>
        protected List<(ModuleHostBuilderMiddlewareType, Action<IModuleHostBuilder, IServiceCollection>)> Middlewares { get; } = new List<(ModuleHostBuilderMiddlewareType, Action<IModuleHostBuilder, IServiceCollection>)>();

        /// <inheritdoc />
        public virtual IReadOnlyList<IModule> Modules => Descriptors.Values.ToArray();

        /// <inheritdoc />
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

        /// <inheritdoc />
        public virtual IModule? GetModule(Type type)
        {
            if (Descriptors.TryGetValue(type, out var value))
                return value;
            else
                return null;
        }

        /// <summary>
        /// Invoked by <see cref="Build(IServiceCollection)"/> before building.
        /// </summary>
        /// <param name="services"></param>
        protected virtual void PreBuild(IServiceCollection services)
        {
            foreach (var middleware in
                from x in Middlewares where x.Item1 is ModuleHostBuilderMiddlewareType.Pre select x.Item2)
            {
                middleware(this, services);
            }
        }

        /// <summary>
        /// Invoked by <see cref="Build(IServiceCollection)"/> after building.
        /// </summary>
        /// <param name="services"></param>
        protected virtual void PostBuild(IServiceCollection services)
        {
            foreach (var middleware in
                from x in Middlewares where x.Item1 is ModuleHostBuilderMiddlewareType.Post select x.Item2)
            {
                middleware(this, services);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IModuleHostBuilder UseMiddleware(ModuleHostBuilderMiddlewareType type, Action<IModuleHostBuilder, IServiceCollection> middleware)
        {
            Middlewares.Add((type, middleware));
            return this;
        }
    }
}