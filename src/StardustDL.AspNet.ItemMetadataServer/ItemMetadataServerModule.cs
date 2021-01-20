using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace StardustDL.AspNet.ItemMetadataServer
{
    [Module(Description = "Provide Item Metadata Server services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class ItemMetadataServerModule : Module<ModuleService, ModuleOption>
    {
        public ItemMetadataServerModule() : base()
        {
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);

            services.AddScoped(typeof(ItemMetadataDomain<>));

            var options = GetSetupOptions(new ModuleOption());

            services.AddDbContext<Data.DataDbContext>(o =>
            {
                if (options.ConfigureDbContext is not null)
                    options.ConfigureDbContext(o);
            });
        }
    }
}
