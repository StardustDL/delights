using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System;
using System.Threading.Tasks;

namespace Test.Modulights
{
    class ModuleTestContext
    {
        public IModuleHostBuilder Builder { get; }

        ModuleTestContext()
        {
            Builder = ModuleHostBuilder.CreateDefaultBuilder();
        }

        public static ModuleTestContext Create()
        {
            return new ModuleTestContext();
        }

        public ModuleTestContext WithModule<T>() where T : IModule
        {
            Builder.AddModule<T>();
            return this;
        }

        public ModuleTestContext WithPlugin<T>() where T : IModuleHostBuilderPlugin
        {
            Builder.UsePlugin<T>();
            return this;
        }

        public IModuleHost Build()
        {
            var services = new ServiceCollection();
            Builder.Build(services);
            return services.BuildServiceProvider().GetModuleHost();
        }

        public async Task UseHostAsync(Func<IModuleHost, Task> action = null)
        {
            var host = Build();
            if (action is not null)
                await action(host);
        }

        public void UseHost(Action<IModuleHost> action = null)
        {
            var host = Build();
            if (action is not null)
                action(host);
        }

        public async Task Run(Action<IModuleHost> action = null, Action<IModuleHost> afterAction = null)
        {
            await UseHostAsync(async host =>
            {
                await using (var _ = await host.Services.UseModuleHost())
                {
                    if (action is not null)
                        action(host);
                }
                if (afterAction is not null)
                    afterAction(host);
            });
        }

        public async Task RunAsync(Func<IModuleHost, Task> action = null, Action<IModuleHost> afterAction = null)
        {
            await UseHostAsync(async host =>
            {
                await using (var _ = await host.Services.UseModuleHost())
                {
                    if (action is not null)
                        await action(host);
                }
                if (afterAction is not null)
                    afterAction(host);
            });
        }
    }
}
