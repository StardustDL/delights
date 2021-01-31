using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StardustDL.AspNet.IdentityServer.Data;
using StardustDL.AspNet.IdentityServer.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StardustDL.AspNet.IdentityServer
{
    public class IdentityServerService
    {
        public IdentityServerService(IServiceProvider services, IdentityDbContext dbContext,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IdentityServerTools identityServerTools, IOptions<IdentityServerModuleOption> options)
        {
            Services = services;
            DbContext = dbContext;
            UserManager = userManager;
            IdentityServerTools = identityServerTools;
            SignInManager = signInManager;
            Options = options.Value;
        }

        public IdentityServerTools IdentityServerTools { get; }

        public IServiceProvider Services { get; }

        public IdentityDbContext DbContext { get; }

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
    }
}
