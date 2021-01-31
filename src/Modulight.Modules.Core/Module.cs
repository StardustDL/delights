using Microsoft.Extensions.Logging;
using Modulight.Modules.Hosting;
using System;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    /// <summary>
    /// Specifies the contract for modules.
    /// </summary>
    public interface IModule
    {
        Task Initialize();

        Task Shutdown();

        ModuleManifest Manifest { get; }
    }

    public abstract class Module : IModule
    {
        Lazy<ModuleManifest> _manifest;

        public IModuleHost Host { get; }

        public IServiceProvider Services { get; }

        protected Module(IModuleHost host)
        {
            Host = host;
            Services = host.Services;
            _manifest = new Lazy<ModuleManifest>(() => Host.GetManifest(GetType()));
        }

        public ModuleManifest Manifest => _manifest.Value;


        public T GetService<T>(IServiceProvider provider) where T : notnull => Host.GetService<T>(provider, GetType());

        public T GetOption<T>(IServiceProvider provider) where T : class => Host.GetOption<T>(provider, GetType());

        public virtual Task Initialize() => Task.CompletedTask;

        public virtual Task Shutdown() => Task.CompletedTask;
    }

    public abstract class Module<TModule> : Module
    {
        protected Module(IModuleHost host) : base(host)
        {
            Logger = host.GetLogger<TModule>();
        }

        public ILogger<TModule> Logger { get; }
    }
}
