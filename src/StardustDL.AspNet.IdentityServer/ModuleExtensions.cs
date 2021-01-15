using System;

namespace StardustDL.AspNet.IdentityServer
{
    public static class ModuleExtensions
    {
        public static Modulight.Modules.IModuleHostBuilder AddIdentityServerModule(this Modulight.Modules.IModuleHostBuilder modules, Action<IdentityServerModuleOption>? setupOptions = null, Action<IdentityServerModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<IdentityServerModule, IdentityServerModuleOption>(setupOptions, configureOptions);
            return modules;
        }

        public static Modulight.Modules.IModuleHostBuilder AddIdentityServerGraphQLModule(this Modulight.Modules.IModuleHostBuilder modules, Action<IdentityServerGraphQLModuleOption>? setupOptions = null, Action<IdentityServerGraphQLModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<IdentityServerGraphQLModule, IdentityServerGraphQLModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }
}
