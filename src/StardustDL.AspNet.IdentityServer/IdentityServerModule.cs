using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;
using StardustDL.AspNet.IdentityServer.Data;
using StardustDL.AspNet.IdentityServer.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace StardustDL.AspNet.IdentityServer
{
    [Module(Description = "Provide Identity Server services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [ModuleService(typeof(IdentityServerService))]
    [ModuleStartup(typeof(Startup))]
    [ModuleOption(typeof(IdentityServerModuleOption))]
    public class IdentityServerModule : AspNetServerModule
    {
        public IdentityServerModule(IModuleHost host, IOptions<IdentityServerModuleOption> options) : base(host)
        {
            Options = options.Value;
        }

        public IdentityServerModuleOption Options { get; }

        public override async Task Initialize()
        {
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await dbContext.Database.EnsureCreatedAsync();
            if (!await dbContext.Users.AnyAsync())
            {
                var result = await userManager.CreateAsync(Options.InitialUser, Options.InitialUserPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Create default user failed.");
                }
            }

            await dbContext.SaveChangesAsync();
            await base.Initialize().ConfigureAwait(false);
        }

        public override void UseMiddleware(IApplicationBuilder builder)
        {
            base.UseMiddleware(builder);

            builder.UseIdentityServer();

            builder.UseAuthentication();
            builder.UseAuthorization();
        }
    }

    public class Startup : ModuleStartup
    {
        public Startup(IOptions<IdentityServerModuleStartupOption> options) => Options = options.Value;

        IdentityServerModuleStartupOption Options { get; }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.IdentityDbContext>(o =>
            {
                if (Options.ConfigureDbContext is not null)
                    Options.ConfigureDbContext(o);
            });

            services.AddDefaultIdentity<ApplicationUser>(o =>
            {
                if (Options.ConfigureIdentity is not null)
                    Options.ConfigureIdentity(o);
            })
                .AddEntityFrameworkStores<Data.IdentityDbContext>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // avoid ms-default mapping, it will change sub claim's type name

            services.AddIdentityServer(o =>
            {
                if (Options.ConfigureIdentityServer is not null)
                    Options.ConfigureIdentityServer(o);
            })
                .AddApiAuthorization<ApplicationUser, Data.IdentityDbContext>(o =>
                {
                    if (Options.ConfigureApiAuthorization is not null)
                        Options.ConfigureApiAuthorization(o);
                });

            services.AddAuthentication().AddIdentityServerJwt();

            base.ConfigureServices(services);
        }
    }
}
