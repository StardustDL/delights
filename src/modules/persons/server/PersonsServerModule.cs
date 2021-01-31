using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;
using StardustDL.AspNet.ItemMetadataServer;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using Delights.Modules.Persons.Server.Models;
using Delights.Modules.Persons.Server.Models.Actions;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Modulight.Modules.Hosting;
using Delights.Modules.Persons.Server.Data;
using System.Threading.Tasks;

namespace Delights.Modules.Persons.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(ModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ItemMetadataServerModule))]
    [GraphQLModuleType("Persons", typeof(ModuleQuery), MutationType = typeof(ModuleMutation))]
    public class PersonsServerModule : GraphQLServerModule<PersonsServerModule>
    {
        public PersonsServerModule(IModuleHost host) : base(host)
        {
        }

        public override async Task Initialize()
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.SaveChangesAsync();
            await base.Initialize();
        }
    }

    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddPersonsServerModule(this IModuleHostBuilder builder, Action<PersonsServerModuleStartupOption, IServiceProvider>? configureStartupOptions = null)
        {
            builder.AddModule<PersonsServerModule>();
            if (configureStartupOptions is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.AddOptions<PersonsServerModuleStartupOption>().Configure(configureStartupOptions);
                });
            }

            return builder;
        }
    }

    public class PersonsServerModuleStartupOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }

    class Startup : ModuleStartup
    {
        public Startup(IOptions<PersonsServerModuleStartupOption> options) => Options = options.Value;

        PersonsServerModuleStartupOption Options { get; }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.DataDbContext>(o =>
            {
                if (Options.ConfigureDbContext is not null)
                    Options.ConfigureDbContext(o);
            });
            base.ConfigureServices(services);
        }
    }

    public class ModuleQuery : Modules.Server.Data.GraphQL.QueryType<PersonsServerModule, ModuleService, RawPerson, Person, PersonMutation>
    {
    }

    public class ModuleMutation : Modules.Server.Data.GraphQL.MutationType<PersonsServerModule, ModuleService, RawPerson, Person, PersonMutation>
    {
    }
}
