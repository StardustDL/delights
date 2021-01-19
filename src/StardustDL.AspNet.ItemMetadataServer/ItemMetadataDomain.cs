using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public class ItemMetadataDomain<T>
    {
        string DomainName => typeof(T).FullName ?? "";

        ItemMetadataServerService Service { get; }

        public ItemMetadataDomain(ItemMetadataServerService service)
        {
            Service = service;
        }

        public async Task<Tag> AddTag(TagMutation value)
        {
            return await Service.AddTag(value with
            {
                Domain = DomainName
            });
        }

        public async Task<Tag?> UpdateTag(TagMutation value)
        {
            var entity = await Service.GetTag(value.Id);
            if (entity is not null && entity.Domain == DomainName)
            {
                return await Service.UpdateTag(value with
                {
                    Domain = DomainName
                });
            }
            return null;
        }

        public async Task<Tag?> RemoveTag(string id)
        {
            var entity = await Service.GetTag(id);
            if (entity is not null && entity.Domain == DomainName)
            {
                return await Service.RemoveTag(id);
            }
            return null;
        }

        public async Task<Item> AddItem(ItemMetadataMutation value)
        {
            return await Service.AddItem(value with
            {
                Domain = DomainName
            });
        }

        public async Task<Item?> UpdateItem(ItemMetadataMutation value)
        {
            var entity = await Service.GetItem(value.Id);
            if (entity is not null && entity.Domain == DomainName)
            {
                return await Service.UpdateItem(value with
                {
                    Domain = DomainName
                });
            }
            return null;
        }

        public async Task<Item?> RemoveItem(string id)
        {
            var entity = await Service.GetItem(id);
            if (entity is not null && entity.Domain == DomainName)
            {
                return await Service.RemoveItem(id);
            }
            return null;
        }

        public async Task<Category> AddCategory(CategoryMutation value)
        {
            return await Service.AddCategory(value with
            {
                Domain = DomainName
            });
        }

        public async Task<Category?> UpdateCategory(CategoryMutation value)
        {
            var entity = await Service.GetCategory(value.Id);
            if (entity is not null && entity.Domain == DomainName)
            {
                return await Service.UpdateCategory(value with
                {
                    Domain = DomainName
                });
            }
            return null;
        }

        public async Task<Category?> RemoveCategory(string id)
        {
            var entity = await Service.GetCategory(id);
            if (entity is not null && entity.Domain == DomainName)
            {
                return await Service.RemoveCategory(id);
            }
            return null;
        }
    }
}
