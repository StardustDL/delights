using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Modulight.Modules.Hosting;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    public sealed class GraphQLServerModulePlugin : ModuleHostBuilderPlugin
    {
        /// <inheritdoc/>
        public override void AfterBuild(IReadOnlyDictionary<Type, ModuleManifest> modules, IServiceCollection services)
        {
            services.AddSingleton<IGraphQLServerModuleHost>(sp => new GraphQLServerModuleHost(sp, modules));
            base.AfterBuild(modules, services);
        }

        public override void AfterModule(Type module, ModuleManifest manifest, IModuleStartup? startup, IServiceCollection services)
        {
            if (module.IsModule<IGraphQLServerModule>())
            {
                GraphQLModuleTypeAttribute? attribute = module.GetCustomAttribute<GraphQLModuleTypeAttribute>();
                if (attribute is not null)
                {
                    var builder = services.AddGraphQLServer(attribute.SchemaName);
                    builder.AddQueryType(attribute.QueryType);
                    if (attribute.MutationType is not null)
                    {
                        builder.AddMutationType(attribute.MutationType);
                    }
                    if (attribute.SubscriptionType is not null)
                    {
                        builder.AddSubscriptionType(attribute.SubscriptionType);
                    }
                    builder.AddFiltering().AddSorting().AddProjections();
                }
            }
            base.AfterModule(module, manifest, startup, services);
        }
    }
}
