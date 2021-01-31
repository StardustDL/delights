using IdentityServer4.Configuration;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace StardustDL.AspNet.IdentityServer
{
    public class IdentityServerModuleStartupOption
    {
        public Action<IdentityOptions>? ConfigureIdentity { get; set; }

        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

        public Action<IdentityServerOptions>? ConfigureIdentityServer { get; set; }

        public Action<ApiAuthorizationOptions>? ConfigureApiAuthorization { get; set; }

    }
}
