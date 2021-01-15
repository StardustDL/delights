namespace Modulight.Modules.Server.GraphQL
{
    public interface IGraphQLServerModuleOption
    {
        /// <summary>
        /// SchemaName, if empty, default to Manifest.Name
        /// </summary>
        string SchemaName { get; set; }

        /// <summary>
        /// Endpoint route, if empty, default to /graphql/{SchemaName}
        /// </summary>
        string Endpoint { get; set; }
    }
}
