using Delights.Modules.Client;
using Delights.Modules.Notes.GraphQL;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;

namespace Delights.Modules.Notes
{
    /// <summary>
    /// Client module for notes.
    /// </summary>
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(ModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ClientModule))]
    [ModuleUIRootPath("notes")]
    public class NotesModule : RazorComponentClientModule
    {
        public NotesModule(IModuleHost host) : base(host)
        {
        }

        public override RenderFragment Icon => Fragments.Icon;
    }

    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddNotesModule(this IModuleHostBuilder builder, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            builder.AddModule<NotesModule>();
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
                "NotesGraphQLClient", (sp, client) =>
                {
                    var option = sp.GetRequiredService<IOptions<ModuleOption>>().Value;
                    client.BaseAddress = new Uri(option.GraphQLEndpoint.TrimEnd('/') + $"/Notes");
                });
            services.AddNotesGraphQLClient();
            base.ConfigureServices(services);
        }
    }

    public class ModuleOption
    {
        public string GraphQLEndpoint { get; set; } = "";
    }

    class ModuleService
    {
        public INotesGraphQLClient GraphQLClient { get; }

        public UrlGenerator UrlGenerator { get; }

        public ModuleService(INotesGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
            UrlGenerator = new UrlGenerator();
        }
    }
}
