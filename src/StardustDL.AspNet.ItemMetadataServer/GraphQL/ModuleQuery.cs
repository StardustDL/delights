using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using StardustDL.AspNet.ItemMetadataServer.Models;
using System.Linq;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Item> GetItems([Service] ItemMetadataServer.ModuleService service)
        {
            return service.QueryAllItems();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Category> GetCategories([Service] ItemMetadataServer.ModuleService service)
        {
            return service.QueryAllCategories();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Tag> GetTags([Service] ItemMetadataServer.ModuleService service)
        {
            return service.QueryAllTags();
        }
    }
}
