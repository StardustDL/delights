using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using StardustDL.AspNet.ItemMetadataServer.Data;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    [Module(Description = "Provide Item Metadata Server services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [ModuleStartup(typeof(Startup))]
    [ModuleService(typeof(ModuleService))]
    public class ItemMetadataServerModule : Module<ItemMetadataServerModule>
    {
        public ItemMetadataServerModule(IModuleHost host) : base(host)
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

    public class Startup : ModuleStartup
    {
        public Startup(IOptions<ItemMetadataServerModuleStartupOption> options) => Options = options.Value;

        ItemMetadataServerModuleStartupOption Options { get; }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ItemMetadataDomain<>));

            services.AddDbContext<Data.DataDbContext>(o =>
            {
                if (Options.ConfigureDbContext is not null)
                    Options.ConfigureDbContext(o);
            });
            base.ConfigureServices(services);
        }
    }
}
