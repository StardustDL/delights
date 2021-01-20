using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using System.Linq;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawItem> GetItems([Service] ItemMetadataServer.ModuleService service)
        {
            return service.QueryAllItems();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawCategory> GetCategories([Service] ItemMetadataServer.ModuleService service)
        {
            return service.QueryAllCategories();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawTag> GetTags([Service] ItemMetadataServer.ModuleService service)
        {
            return service.QueryAllTags();
        }
    }
}
