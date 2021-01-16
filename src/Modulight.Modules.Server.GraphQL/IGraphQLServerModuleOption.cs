namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Specifies the contract for graphql module options.
    /// </summary>
    public interface IGraphQLServerModuleOption
    {
        /// <summary>
        /// SchemaName
        /// </summary>
        string SchemaName { get; set; }

        /// <summary>
        /// Endpoint route
        /// </summary>
        string Endpoint { get; set; }
    }
}
