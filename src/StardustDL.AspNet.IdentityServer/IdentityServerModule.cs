using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Services;
using StardustDL.AspNet.IdentityServer.Data;
using StardustDL.AspNet.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StardustDL.AspNet.IdentityServer
{
    [Module(Description = "Provide Identity Server services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class IdentityServerModule : AspNetServerModule<IdentityServerService, IdentityServerModuleOption>
    {
        public IdentityServerModule() : base()
        {
        }

        public override void RegisterAspNetServices(IServiceCollection services)
        {
            base.RegisterAspNetServices(services);

            var options = GetSetupOptions(new IdentityServerModuleOption());

            services.AddDbContext<Data.IdentityDbContext>(o =>
            {
                if (options.ConfigureDbContext is not null)
                    options.ConfigureDbContext(o);
            });

            services.AddDefaultIdentity<ApplicationUser>(o =>
            {
                if (options.ConfigureIdentity is not null)
                    options.ConfigureIdentity(o);
            })
                .AddEntityFrameworkStores<Data.IdentityDbContext>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // avoid ms-default mapping, it will change sub claim's type name

            services.AddIdentityServer(o =>
            {
                if (options.ConfigureIdentityServer is not null)
                    options.ConfigureIdentityServer(o);
            })
                .AddApiAuthorization<ApplicationUser, Data.IdentityDbContext>(o =>
                {
                    if (options.ConfigureApiAuthorization is not null)
                        options.ConfigureApiAuthorization(o);
                });

            services.AddAuthentication().AddIdentityServerJwt();
        }

        public override void UseMiddleware(IApplicationBuilder builder, IServiceProvider provider)
        {
            base.UseMiddleware(builder, provider);

            builder.UseIdentityServer();

            builder.UseAuthentication();
            builder.UseAuthorization();
        }
    }

    public class IdentityServerService : IModuleService
    {
        public IdentityServerService(IServiceProvider services, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IdentityServerTools identityServerTools, IOptions<IdentityServerModuleOption> options)
        {
            Services = services;
            UserManager = userManager;
            IdentityServerTools = identityServerTools;
            SignInManager = signInManager;
            Options = options.Value;
        }

        public IdentityServerTools IdentityServerTools { get; }

        public IServiceProvider Services { get; }

        public UserManager<ApplicationUser> UserManager { get; }

        public SignInManager<ApplicationUser> SignInManager { get; }

        public IdentityServerModuleOption Options { get; }

        public async Task<string> GetToken(string userName, string password)
        {
            var user = await UserManager.FindByNameAsync(userName);

            if (user is null)
            {
                throw new Exception($"Not found user by name {userName}.");
            }

            var result = await SignInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                var token = await IdentityServerTools.IssueClientJwtAsync(
                   clientId: "Internal",
                   lifetime: 3600,
                   scopes: new string[] { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile },
                   audiences: Options.JwtAudiences,
                   additionalClaims: new Claim[] {
                       new Claim(JwtClaimTypes.Subject, user.Id),
                       new Claim(JwtClaimTypes.Name, user.UserName),
                   });
                return token;
            }
            else
            {
                throw new Exception($"Failed to login.");
            }
        }

        public async Task Initialize(ApplicationUser firstUser, string firstUserPassword)
        {
            var context = Services.GetRequiredService<Data.IdentityDbContext>();
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();
            if (!await context.Users.AnyAsync())
            {
                var result = await UserManager.CreateAsync(firstUser, firstUserPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Create default user failed.");
                }
            }

            await context.SaveChangesAsync();
        }
    }

    public class IdentityServerModuleOption
    {
        public Action<IdentityOptions>? ConfigureIdentity { get; set; }

        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

        public Action<IdentityServerOptions>? ConfigureIdentityServer { get; set; }

        public Action<ApiAuthorizationOptions>? ConfigureApiAuthorization { get; set; }

        public string[] JwtAudiences { get; set; } = Array.Empty<string>();
    }
}
