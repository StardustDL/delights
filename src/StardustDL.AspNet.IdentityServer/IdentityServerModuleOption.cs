using StardustDL.AspNet.IdentityServer.Models;
using System;

namespace StardustDL.AspNet.IdentityServer
{
    public class IdentityServerModuleOption
    {
        public ApplicationUser InitialUser { get; set; } = new ApplicationUser { UserName = "admin" };

        public string InitialUserPassword { get; set; } = "123456";

        public string[] JwtAudiences { get; set; } = Array.Empty<string>();
    }
}
