using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;
using StardustDL.AspNet.ItemMetadataServer;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using Delights.Modules.Persons.Server.Models;
using Delights.Modules.Persons.Server.Models.Actions;

namespace Delights.Modules.Persons.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class PersonsServerModule : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public override Type? MutationType => typeof(ModuleMutation);

        public PersonsServerModule() : base()
        {
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddItemMetadataServerModule();
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);

            var options = GetSetupOptions(new ModuleOption());

            services.AddDbContext<Data.DataDbContext>(o =>
            {
                if (options.ConfigureDbContext is not null)
                    options.ConfigureDbContext(o);
            });
        }
    }

    public class ModuleQuery : Modules.Server.Data.GraphQL.QueryType<PersonsServerModule, ModuleService, RawPerson, Person, PersonMutation>
    {
    }

    public class ModuleMutation : Modules.Server.Data.GraphQL.MutationType<PersonsServerModule, ModuleService, RawPerson, Person, PersonMutation>
    {
    }
}
