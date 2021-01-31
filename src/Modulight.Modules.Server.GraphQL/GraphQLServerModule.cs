using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Modulight.Modules.Hosting;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Basic implement for <see cref="IGraphQLServerModule"/>
    /// </summary>
    public abstract class GraphQLServerModule<TModule> : Module<TModule>, IGraphQLServerModule
    {
        protected GraphQLServerModule(IModuleHost host) : base(host)
        {
        }

        /// <inheritdoc/>
        public virtual GraphQLEndpointConventionBuilder? MapEndpoint(IEndpointRouteBuilder builder)
        {
            GraphQLModuleTypeAttribute? attribute = GetType().GetCustomAttribute<GraphQLModuleTypeAttribute>();
            if (attribute is not null)
            {
                return builder.MapGraphQL(attribute.Endpoint.TrimEnd('/'), attribute.SchemaName);
            }
            return null;
        }
    }
}
