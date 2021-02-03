using Delights.Modules.Bookkeeping.GraphQL;
using Delights.Modules.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;

namespace Delights.Modules.Bookkeeping
{
    /// <summary>
    /// Client module for bookkeeping.
    /// </summary>
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(ModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ClientModule))]
    [ModuleUIRootPath("bookkeeping")]
    public class BookkeepingModule : RazorComponentClientModule
    {
        public BookkeepingModule(IModuleHost host) : base(host)
        {
        }

        public override RenderFragment Icon => Fragments.Icon;
    }

    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddBookkeepingModule(this IModuleHostBuilder builder, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<BookkeepingModule>();
            if (configureOptions is not null)
            {
                builder.ConfigureOptions(configureOptions);
            }

            return builder;
        }
    }

    class Startup : ModuleStartup
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

    public class ModuleOption
    {
        public string GraphQLEndpoint { get; set; } = "";
    }

    class ModuleService
    {
        public IBookkeepingGraphQLClient GraphQLClient { get; }

        public UrlGenerator UrlGenerator { get; }

        public ModuleService(IBookkeepingGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
            UrlGenerator = new UrlGenerator();
        }
    }
}
