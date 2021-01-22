using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;
using StardustDL.AspNet.ItemMetadataServer;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using Delights.Modules.Bookkeeping.Server.Models;
using Delights.Modules.Bookkeeping.Server.Models.Actions;

namespace Delights.Modules.Bookkeeping.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class BookkeepingServerModule : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public override Type? MutationType => typeof(ModuleMutation);

        public BookkeepingServerModule() : base()
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

    public class ModuleQuery : Modules.Server.Data.GraphQL.QueryType<BookkeepingServerModule, ModuleService, RawAccountItem, AccountItem, AccountItemMutation>
    {
    }

    public class ModuleMutation : Modules.Server.Data.GraphQL.MutationType<BookkeepingServerModule, ModuleService, RawAccountItem, AccountItem, AccountItemMutation>
    {
    }
}
