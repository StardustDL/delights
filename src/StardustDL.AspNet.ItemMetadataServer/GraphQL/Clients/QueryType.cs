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
    public abstract class QueryType<T>
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public virtual IQueryable<RawTag> GetTags([Service] ItemMetadataDomain<T> service)
        {
            return service.QueryAllTags();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public virtual IQueryable<RawCategory> GetCategories([Service] ItemMetadataDomain<T> service)
        {
            return service.QueryAllCategories();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public virtual IQueryable<RawItemMetadata> GetMetadata([Service] ItemMetadataDomain<T> service)
        {
            return service.QueryAllItems();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public virtual IQueryable<RawItemMetadata> GetMetadataByTag(string name, [Service] ItemMetadataDomain<T> service)
        {
            return service.QueryItemsByTag(name);
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public virtual IQueryable<RawItemMetadata> GetMetadataByCategory(string name, [Service] ItemMetadataDomain<T> service)
        {
            return service.QueryItemsByCategory(name);
        }

        public virtual async Task<RawItemMetadata?> GetMetadataByMetadataId(string id, [Service] ItemMetadataDomain<T> service)
        {
            return await service.GetItem(id);
        }

        public virtual async Task<RawCategory?> GetCategory(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.GetCategory(name);
        }

        public virtual async Task<RawTag?> GetTag(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.GetTag(name);
        }
    }
}
