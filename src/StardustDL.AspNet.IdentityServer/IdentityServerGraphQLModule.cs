using HotChocolate;
using HotChocolate.AspNetCore.Extensions;
using HotChocolate.Data;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StardustDL.AspNet.IdentityServer
{
    [Module(Description = "Provide GraphQL endpoints for identity server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [ModuleDependency(typeof(IdentityServerModule))]
    [GraphQLModuleType("IdentityServer", typeof(ModuleQuery))]
    public class IdentityServerGraphqlModule : GraphQLServerModule
    {
    }

    public class ModuleQuery
    {
        public async Task<string> GetToken(string userName, string password, [Service] IdentityServerService service)
        {
            return await service.GetToken(userName, password);
        }

        public string GetUid([Service] IdentityServerService service)
        {
            return service.SignInManager.Context.User.Identity?.GetSubjectId() ?? "";
        }
    }
}
