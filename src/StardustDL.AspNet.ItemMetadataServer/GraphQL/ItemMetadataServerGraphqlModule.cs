using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.GraphQL;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    [Module(Description = "Provide GraphQL endpoints for item metadata server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [GraphQLModuleType("ItemMetadataServer", typeof(ModuleQuery), MutationType = typeof(ModuleMutation))]
    [ModuleDependency(typeof(ItemMetadataServerModule))]
    public class ItemMetadataServerGraphqlModule : GraphQLServerModule
    {
        public ItemMetadataServerGraphqlModule(IModuleHost host) : base(host)
        {
        }
    }
}
