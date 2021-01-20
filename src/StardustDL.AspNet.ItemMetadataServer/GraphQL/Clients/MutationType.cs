using HotChocolate;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL.Clients
{
    public class MutationType<T>
    {
        public async Task<RawTag> CreateTag(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.AddTag(name);
        }

        public async Task<RawCategory> CreateCategory(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.AddCategory(name);
        }

        public async Task<RawTag?> DeleteTag(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.RemoveTag(name);
        }

        public async Task<RawCategory?> DeleteCategory(string name, [Service] ItemMetadataDomain<T> service)
        {
            return await service.RemoveCategory(name);
        }

        public async Task<RawTag?> RenameTag(string oldName, string newName, [Service] ItemMetadataDomain<T> service)
        {
            return await service.RenameTag(oldName, newName);
        }

        public async Task<RawCategory?> RenameCategory(string oldName, string newName, [Service] ItemMetadataDomain<T> service)
        {
            return await service.RenameCategory(oldName, newName);
        }
    }
}
