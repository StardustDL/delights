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
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
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

        public override Type? MutationType => typeof(ModuleMutation);

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
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ItemMetadata> GetItems([Service] ItemMetadataServerService service)
        {
            return service.GetAllItems();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Category> GetCategories([Service] ItemMetadataServerService service)
        {
            return service.GetAllCategories();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tag> GetTags([Service] ItemMetadataServerService service)
        {
            return service.GetAllTags();
        }
    }

    public class ModuleMutation
    {
        public async Task<Tag> CreateTag(TagMutation mutation, [Service] ItemMetadataServerService service)
        {
            return await service.AddTag(mutation);
        }

        public async Task<Category> CreateCategory(CategoryMutation mutation, [Service] ItemMetadataServerService service)
        {
            return await service.AddCategory(mutation);
        }

        public async Task<ItemMetadata> CreateItem(ItemMetadataMutation mutation, [Service] ItemMetadataServerService service)
        {
            return await service.AddItem(mutation);
        }

        public async Task<Tag> DeleteTag(string id, [Service] ItemMetadataServerService service)
        {
            return await service.RemoveTag(id);
        }

        public async Task<Category> DeleteCategory(string id, [Service] ItemMetadataServerService service)
        {
            return await service.RemoveCategory(id);
        }

        public async Task<ItemMetadata> DeleteItem(string id, [Service] ItemMetadataServerService service)
        {
            return await service.RemoveItem(id);
        }

        public async Task<Tag> UpdateTag(TagMutation mutation, [Service] ItemMetadataServerService service)
        {
            return await service.UpdateTag(mutation);
        }

        public async Task<Category> UpdateCategory(CategoryMutation mutation, [Service] ItemMetadataServerService service)
        {
            return await service.UpdateCategory(mutation);
        }

        public async Task<ItemMetadata> UpdateItem(ItemMetadataMutation mutation, [Service] ItemMetadataServerService service)
        {
            return await service.UpdateItem(mutation);
        }
    }

    public class ItemMetadataServerGraphqlModuleService : IModuleService
    {
    }

    public class ItemMetadataServerGraphqlModuleOption : GraphQLServerModuleOption
    {
    }
}
