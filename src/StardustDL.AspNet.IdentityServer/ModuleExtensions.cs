using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System;

namespace StardustDL.AspNet.IdentityServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddIdentityServerModule(this IModuleHostBuilder builder, Action<IdentityServerModuleStartupOption, IServiceProvider>? configureStartupOptions = null)
        {
            builder.AddModule<IdentityServerModule>();
            if (configureStartupOptions is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.AddOptions<IdentityServerModuleStartupOption>().Configure(configureStartupOptions);
                });
            }

            return builder;
        }

        public static IModuleHostBuilder AddIdentityServerGraphqlModule(this IModuleHostBuilder builder)
        {
            builder.AddModule<IdentityServerGraphqlModule>();
            return builder;
        }
    }
}
