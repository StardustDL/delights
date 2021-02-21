using Delights.Modules.Bookkeeping;
using Delights.Modules.Notes;
using Delights.Modules.Persons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules.Client.RazorComponents;
using Modulight.Modules.Hosting;

namespace Delights.Client.Shared
{
    public static class ModuleSetup
    {
        public static IModuleHostBuilder UseDefaults(this IModuleHostBuilder builder)
        {
            builder.ConfigureOptions<ServerConfiguration>((o, _) =>
                 {
                     o.GraphQLEndpoint = "https://localhost:5001/graphql";
                 })
                .AddModule<UI.UiModule>()
            .AddNotesModule(configureOptions: (o, sp) =>
            {
                var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
                o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
            })
        .AddPersonsModule(configureOptions: (o, sp) =>
         {
             var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
             o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
         })
        .AddBookkeepingModule(configureOptions: (o, sp) =>
        {
            var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
            o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
        });

            /*builder.AddHelloModule(configureOptions: (o, sp) =>
             {
                 var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
                 o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
             })
            .AddModuleManagerModule(configureOptions: (o, sp) =>
            {
                var serverConfiguration = sp.GetRequiredService<IOptions<ServerConfiguration>>().Value;
                o.GraphQLEndpoint = serverConfiguration.GraphQLEndpoint;
            });*/

            return builder;
        }
    }
}
