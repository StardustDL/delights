using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL.Clients
{
    public class QueryType<T>
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawTag> GetTags([Service] ItemMetadataDomain<T> service)
        {
            return service.QueryAllTags();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawCategory> GetCategories([Service] ItemMetadataDomain<T> service)
        {
            return service.QueryAllCategories();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawItem> GetMetadata([Service] ItemMetadataDomain<T> service)
        {
            return service.QueryAllItems();
        }

        public async Task<RawItem?> GetMetadataByMetadataId(string id, [Service] ItemMetadataDomain<T> service)
        {
            return await service.GetItem(id);
        }

        public async Task<RawCategory?> GetCategory(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.GetCategory(name);
        }

        public async Task<RawTag?> GetTag(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.GetTag(name);
        }
    }
}
