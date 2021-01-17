using Modulight.Modules;
using System;

namespace StardustDL.AspNet.IdentityServer
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddIdentityServerModule(this IModuleHostBuilder modules, Action<IdentityServerModuleOption>? setupOptions = null, Action<IdentityServerModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<IdentityServerModule, IdentityServerModuleOption>(setupOptions, configureOptions);
            return modules;
        }

        public static IModuleHostBuilder AddIdentityServerGraphQLModule(this IModuleHostBuilder modules, Action<IdentityServerGraphqlModuleOption>? setupOptions = null, Action<IdentityServerGraphqlModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<IdentityServerGraphqlModule, IdentityServerGraphqlModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
