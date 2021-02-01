using HotChocolate;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.GraphQL.Clients
{
    public abstract class MutationType<T>
    {
        public virtual async Task<RawTag> CreateTag(string name, [Service] IItemMetadataDomain<T> service)
        {
            return await service.AddTag(name);
        }

        public virtual async Task<RawCategory> CreateCategory(string name, [Service] IItemMetadataDomain<T> service)
        {
            return await service.AddCategory(name);
        }

        public virtual async Task<RawTag?> DeleteTag(string name, [Service] IItemMetadataDomain<T> service)
        {
            return await service.RemoveTag(name);
        }

        public virtual async Task<RawCategory?> DeleteCategory(string name, [Service] IItemMetadataDomain<T> service)
        {
            return await service.RemoveCategory(name);
        }

        public virtual async Task<RawTag?> RenameTag(string oldName, string newName, [Service] IItemMetadataDomain<T> service)
        {
            return await service.RenameTag(oldName, newName);
        }

        public virtual async Task<RawCategory?> RenameCategory(string oldName, string newName, [Service] IItemMetadataDomain<T> service)
        {
            return await service.RenameCategory(oldName, newName);
        }
    }
}
