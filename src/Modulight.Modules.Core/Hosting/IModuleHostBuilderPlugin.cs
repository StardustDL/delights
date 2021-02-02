using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules.Hosting
{
    /// <summary>
    /// Plugin for <see cref="IModuleHostBuilder"/>.
    /// </summary>
    public interface IModuleHostBuilderPlugin
    {
        /// <summary>
        /// Do after the host builded.
        /// </summary>
        /// <param name="modules"></param>
        /// <param name="services"></param>
        void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services);

        /// <summary>
        /// Do after the module registered.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="manifest"></param>
        /// <param name="startup"></param>
        /// <param name="services"></param>
        void AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services);

        /// <summary>
        /// Do before the host is building.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="services"></param>
        void BeforeBuild(IModuleHostBuilder builder, IServiceCollection services);

        /// <summary>
        /// Do before the module is registering.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="manifest"></param>
        /// <param name="startup"></param>
        /// <param name="services"></param>
        void BeforeModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services);
    }

    /// <summary>
    /// Empty implementation for <see cref="IModuleHostBuilderPlugin"/>.
    /// </summary>
    public abstract class ModuleHostBuilderPlugin : IModuleHostBuilderPlugin
    {
        /// <inheritdoc/>
        public virtual void BeforeBuild(IModuleHostBuilder builder, IServiceCollection services) { }

        /// <inheritdoc/>
        public virtual void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services) { }

        /// <inheritdoc/>
        public virtual void BeforeModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services) { }

        /// <inheritdoc/>
        public virtual void AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services) { }
    }
}