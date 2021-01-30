using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Modulight.Modules.Hosting;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the contract for modules.
    /// </summary>
    public interface IModule
    {
        Task Initialize();

        Task Shutdown();
    }

    public abstract class Module : IModule
    {
        public virtual Task Initialize() => Task.CompletedTask;

        public virtual Task Shutdown() => Task.CompletedTask;
    }

    public abstract class Module<TModule> : Module
    {
        Lazy<ModuleManifest> _manifest;

        protected Module(IModuleHost host)
        {
            Logger = host.GetLogger<TModule>();
            Host = host;
            Services = host.Services;
            _manifest = new Lazy<ModuleManifest>(() => Host.GetManifest(this));
        }

        public ILogger<TModule> Logger { get; }

        public IModuleHost Host { get; }

        public IServiceProvider Services { get; }

        public T GetService<T>(IServiceProvider provider) where T : notnull => Host.GetService<T>(provider, this);

        public T GetOption<T>(IServiceProvider provider) where T : class => Host.GetOption<T>(provider, this);

        public ModuleManifest Manifest => _manifest.Value;
    }
}
