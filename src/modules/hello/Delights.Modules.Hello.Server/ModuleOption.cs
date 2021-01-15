using Modulight.Modules.Server.GraphQL;

namespace Delights.Modules.Hello.Server
{
    public class ModuleOption : IGraphQLServerModuleOption
    {
        public string SchemaName { get; set; } = "";

        public string Endpoint { get; set; } = "";
    }
}
