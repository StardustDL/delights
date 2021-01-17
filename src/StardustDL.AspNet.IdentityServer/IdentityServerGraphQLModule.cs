using HotChocolate;
using HotChocolate.AspNetCore.Extensions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Server.GraphQL;
using Modulight.Modules.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StardustDL.AspNet.IdentityServer
{
    [Module(Description = "Provide GraphQL endpoints for identity server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class IdentityServerGraphqlModule : GraphQLServerModule<IdentityServerGraphqlModuleService, IdentityServerGraphqlModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public IdentityServerGraphqlModule() : base()
        {
        }

        public override GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            return base.MapEndpoint(builder, provider);
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddIdentityServerModule();
        }
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

    public class IdentityServerGraphqlModuleService : IModuleService
    {
    }

    public class IdentityServerGraphqlModuleOption : GraphQLServerModuleOption
    {
    }
}
