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
        protected Module(IModuleHost host)
        {
            Logger = host.GetLogger<TModule>();
            Host = host;
        }

        protected ILogger<TModule> Logger { get; }

        protected IModuleHost Host { get; }

        protected T GetService<T>(IServiceProvider provider) where T : notnull => Host.GetService<T>(provider, this);

        protected T GetOption<T>(IServiceProvider provider) where T : class => Host.GetOption<T>(provider, this);
    }
}
