using HotChocolate;
using HotChocolate.AspNetCore.Extensions;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Server.GraphQL;
using Modulight.Modules.Services;
using StardustDL.AspNet.ItemMetadataServer.Data;
using StardustDL.AspNet.ItemMetadataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    [Module(Description = "Provide GraphQL endpoints for item metadata server.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class ItemMetadataServerGraphqlModule : GraphQLServerModule<ItemMetadataServerGraphqlModuleService, ItemMetadataServerGraphqlModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public ItemMetadataServerGraphqlModule() : base()
        {
        }

        public override GraphQLEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder builder, IServiceProvider provider)
        {
            return base.MapEndpoint(builder, provider);
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddItemMetadataServerModule();
        }
    }

    public class ModuleQuery
    {
        [UseDbContext(typeof(ItemMetadataDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ItemMetadata> GetItems([ScopedService] ItemMetadataDbContext context)
        {
            return context.Items;
        }

        [UseDbContext(typeof(ItemMetadataDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Category> GetCategories([ScopedService] ItemMetadataDbContext context)
        {
            return context.Categories;
        }

        [UseDbContext(typeof(ItemMetadataDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Domain> GetDomains([ScopedService] ItemMetadataDbContext context)
        {
            return context.Domains;
        }

        [UseDbContext(typeof(ItemMetadataDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tag> GetTags([ScopedService] ItemMetadataDbContext context)
        {
            return context.Tags;
        }
    }

    public class ItemMetadataServerGraphqlModuleService : IModuleService
    {
    }

    public class ItemMetadataServerGraphqlModuleOption : GraphQLServerModuleOption
    {
    }
}
