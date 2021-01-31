using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;
using StardustDL.AspNet.ItemMetadataServer;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using Delights.Modules.Notes.Server.Models;
using Delights.Modules.Notes.Server.Models.Actions;
using Modulight.Modules.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Delights.Modules.Notes.Server.Data;

namespace Delights.Modules.Notes.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [ModuleService(typeof(ModuleService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleDependency(typeof(ItemMetadataServerModule))]
    [GraphQLModuleType("Notes", typeof(ModuleQuery), MutationType = typeof(ModuleMutation))]
    public class NotesServerModule : GraphQLServerModule<NotesServerModule>
    {
        public NotesServerModule(IModuleHost host) : base(host)
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
        public static IModuleHostBuilder AddNotesServerModule(this IModuleHostBuilder builder, Action<NotesServerModuleStartupOption, IServiceProvider>? configureStartupOptions = null)
        {
            builder.AddModule<NotesServerModule>();
            if (configureStartupOptions is not null)
            {
                builder.ConfigureBuilderServices(services =>
                {
                    services.AddOptions<NotesServerModuleStartupOption>().Configure(configureStartupOptions);
                });
            }

            return builder;
        }
    }

    public class NotesServerModuleStartupOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }

    class Startup : ModuleStartup
    {
        public Startup(IOptions<NotesServerModuleStartupOption> options) => Options = options.Value;

        NotesServerModuleStartupOption Options { get; }

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

    public class ModuleQuery : Modules.Server.Data.GraphQL.QueryType<NotesServerModule, ModuleService, RawNote, Note, NoteMutation>
    {
    }

    public class ModuleMutation : Modules.Server.Data.GraphQL.MutationType<NotesServerModule, ModuleService, RawNote, Note, NoteMutation>
    {
    }
}
