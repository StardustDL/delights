using HotChocolate;
using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    public class ModuleMutation
    {
        public async Task<Tag> CreateTag(TagMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.AddTag(mutation);
        }

        public async Task<Category> CreateCategory(CategoryMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.AddCategory(mutation);
        }

        public async Task<Item> CreateItem(ItemMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.AddItem(mutation);
        }

        public async Task<Tag?> DeleteTag(string id, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.RemoveTag(id);
        }

        public async Task<Category?> DeleteCategory(string id, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.RemoveCategory(id);
        }

        public async Task<Item?> DeleteItem(string id, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.RemoveItem(id);
        }

        public async Task<Tag?> UpdateTag(TagMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.UpdateTag(mutation);
        }

        public async Task<Category?> UpdateCategory(CategoryMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.UpdateCategory(mutation);
        }

        public async Task<Item?> UpdateItem(ItemMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.UpdateItem(mutation);
        }
    }
}
