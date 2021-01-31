using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules.Hosting
{
    public interface IModuleHostBuilderPlugin
    {
        void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services);
        void AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services);
        void BeforeBuild(IModuleHostBuilder builder, IServiceCollection services);
        void BeforeModule(Type module, ModuleManifest manifest, IServiceCollection services);
    }

    public abstract class ModuleHostBuilderPlugin : IModuleHostBuilderPlugin
    {
        public virtual void BeforeBuild(IModuleHostBuilder builder, IServiceCollection services) { }

        public virtual void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services) { }

        public virtual void BeforeModule(Type module, ModuleManifest manifest, IServiceCollection services) { }

        public virtual void AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services) { }
    }
}