using Delights.Modules.Notes.Server.Data;
using Delights.Modules.Notes.Server.Models;
using Delights.Modules.Notes.Server.Models.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;
using StardustDL.AspNet.ItemMetadataServer;
using System;
using System.Threading.Tasks;

namespace Delights.Modules.Notes.Server
{
    /// <summary>
    /// Server module for notes.
    /// </summary>
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
                builder.ConfigureBuilderOptions(configureStartupOptions);
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
