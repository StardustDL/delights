using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Routing;
using HotChocolate.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    /// <summary>
    /// Basic implement for <see cref="IGraphQLServerModule"/>
    /// </summary>
    public abstract class GraphQLServerModule : Module, IGraphQLServerModule
    {
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
