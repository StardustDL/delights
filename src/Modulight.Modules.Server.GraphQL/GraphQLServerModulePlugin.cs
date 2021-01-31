using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Hosting;
using System;
using System.Reflection;

namespace Modulight.Modules.Server.GraphQL
{
    internal sealed class GraphQLServerModulePlugin : ModuleHostBuilderPlugin
    {
        public override void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services)
        {
            services.AddSingleton<IGraphQLServerModuleCollection>(sp => new GraphQLServerModuleCollection(sp.GetRequiredService<IModuleHost>()));
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
