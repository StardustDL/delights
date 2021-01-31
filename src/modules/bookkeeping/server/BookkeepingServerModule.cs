using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;
using StardustDL.AspNet.ItemMetadataServer;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using Delights.Modules.Bookkeeping.Server.Models;
using Delights.Modules.Bookkeeping.Server.Models.Actions;
using Modulight.Modules.Hosting;
using System.Threading.Tasks;
using Delights.Modules.Bookkeeping.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Delights.Modules.Bookkeeping.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(ModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ItemMetadataServerModule))]
    [GraphQLModuleType("Bookkeeping", typeof(ModuleQuery), MutationType = typeof(ModuleMutation))]
    public class BookkeepingServerModule : GraphQLServerModule<BookkeepingServerModule>
    {
        public BookkeepingServerModule(IModuleHost host) : base(host)
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
        public static IModuleHostBuilder AddBookkeepingServerModule(this IModuleHostBuilder builder, Action<BookkeepingServerModuleStartupOption, IServiceProvider>? configureStartupOptions = null)
        {
            builder.AddModule<BookkeepingServerModule>();
            if (configureStartupOptions is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.AddOptions<BookkeepingServerModuleStartupOption>().Configure(configureStartupOptions);
                });
            }

            return builder;
        }
    }

    public class BookkeepingServerModuleStartupOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }

    class Startup : ModuleStartup
    {
        public Startup(IOptions<BookkeepingServerModuleStartupOption> options) => Options = options.Value;

        BookkeepingServerModuleStartupOption Options { get; }

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

    public class ModuleQuery : Modules.Server.Data.GraphQL.QueryType<BookkeepingServerModule, ModuleService, RawAccountItem, AccountItem, AccountItemMutation>
    {
    }

    public class ModuleMutation : Modules.Server.Data.GraphQL.MutationType<BookkeepingServerModule, ModuleService, RawAccountItem, AccountItem, AccountItemMutation>
    {
    }
}
