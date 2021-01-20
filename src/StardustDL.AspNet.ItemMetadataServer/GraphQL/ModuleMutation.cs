using HotChocolate;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL
{
    public class ModuleMutation
    {
        public async Task<RawTag> CreateTag(RawTagMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.AddTag(mutation);
        }

        public async Task<RawCategory> CreateCategory(RawCategoryMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.AddCategory(mutation);
        }

        public async Task<RawItem> CreateItem(RawItemMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.AddItem(mutation);
        }

        public async Task<RawTag?> DeleteTag(string id, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.RemoveTag(id);
        }

        public async Task<RawCategory?> DeleteCategory(string id, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.RemoveCategory(id);
        }

        public async Task<RawItem?> DeleteItem(string id, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.RemoveItem(id);
        }

        public async Task<RawTag?> UpdateTag(RawTagMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.UpdateTag(mutation);
        }

        public async Task<RawCategory?> UpdateCategory(RawCategoryMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.UpdateCategory(mutation);
        }

        public async Task<RawItem?> UpdateItem(RawItemMutation mutation, [Service] ItemMetadataServer.ModuleService service)
        {
            return await service.UpdateItem(mutation);
        }
    }
}
