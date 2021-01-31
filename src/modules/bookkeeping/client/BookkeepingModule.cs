using Modulight.Modules.Client.RazorComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delights.Modules.Bookkeeping.GraphQL;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Delights.Modules.Client;
using Modulight.Modules.Hosting;

namespace Delights.Modules.Bookkeeping
{

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(ModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ClientModule))]
    [ModuleUI(typeof(ModuleUI))]
    public class BookkeepingModule : RazorComponentClientModule<BookkeepingModule>
    {
        public BookkeepingModule(IModuleHost host) : base(host)
        {
        }
    }

    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddBookkeepingModule(this IModuleHostBuilder builder, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<BookkeepingModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureServices(services =>
                {
                    services.AddOptions<ModuleOption>().Configure(configureOptions);
                });
            }

            return builder;
        }
    }

    public class Startup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(
                "BookkeepingGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/Bookkeeping");
                });
            services.AddBookkeepingGraphQLClient();
            base.ConfigureServices(services);
        }
    }
}
