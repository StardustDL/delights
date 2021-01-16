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

    /// <summary>
    /// Basic implement for <see cref="IGraphQLServerModuleOption"/>.
    /// </summary>
    public class GraphQLServerModuleOption : IGraphQLServerModuleOption
    {
        /// <inheritdoc/>
        public string SchemaName { get; set; } = "";

        /// <inheritdoc/>
        public string Endpoint { get; set; } = "";
    }
}
