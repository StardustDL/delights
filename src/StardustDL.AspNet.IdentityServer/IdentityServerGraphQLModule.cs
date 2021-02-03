using HotChocolate;
using IdentityServer4.Extensions;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;
using System.Threading.Tasks;

namespace StardustDL.AspNet.IdentityServer
{
    [Module(Description = "Provide GraphQL endpoints for identity server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [ModuleDependency(typeof(IdentityServerModule))]
    [GraphQLModuleType("IdentityServer", typeof(ModuleQuery))]
    public class IdentityServerGraphqlModule : GraphQLServerModule
    {
        public IdentityServerGraphqlModule(IModuleHost host) : base(host)
        {
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
}
