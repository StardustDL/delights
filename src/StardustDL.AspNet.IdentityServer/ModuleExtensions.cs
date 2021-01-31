using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;

namespace StardustDL.AspNet.IdentityServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddIdentityServerModule(this IModuleHostBuilder builder, Action<IdentityServerModuleStartupOption, IServiceProvider>? configureStartupOptions = null, Action<IdentityServerModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<IdentityServerModule>();
            if (configureStartupOptions is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.AddOptions<IdentityServerModuleStartupOption>().Configure(configureStartupOptions);
                });
            }
            if (configureOptions is not null)
            {
                builder.ConfigureServices(services =>
                {
                    services.AddOptions<IdentityServerModuleOption>().Configure(configureOptions);
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
