using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulight.Modules.Hosting
{
    public interface IModuleHostBuilderPlugin
    {
        Task AfterBuild(IReadOnlyDictionary<Type, ModuleManifest> modules, IServiceCollection services);
        Task AfterModule(Type module, ModuleManifest manifest, IServiceCollection services);
        Task BeforeBuild(IModuleHostBuilder builder, IServiceCollection services);
        Task BeforeModule(Type module, ModuleManifest manifest, IServiceCollection services);
    }

    public abstract class ModuleHostBuilderPlugin : IModuleHostBuilderPlugin
    {
        public virtual Task BeforeBuild(IModuleHostBuilder builder, IServiceCollection services) => Task.CompletedTask;

        public virtual Task AfterBuild(IReadOnlyDictionary<Type, ModuleManifest> modules, IServiceCollection services) => Task.CompletedTask;

        public virtual Task BeforeModule(Type module, ModuleManifest manifest, IServiceCollection services) => Task.CompletedTask;

        public virtual Task AfterModule(Type module, ModuleManifest manifest, IServiceCollection services) => Task.CompletedTask;
    }
}