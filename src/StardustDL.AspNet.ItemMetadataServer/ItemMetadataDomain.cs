using Microsoft.EntityFrameworkCore;
using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System.Linq;
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

        public IQueryable<Item> GetAllItems()
        {
            return Service.GetAllItems().Where(x => x.Domain == DomainName);
        }

        public IQueryable<Category> GetAllCategories()
        {
            return Service.GetAllCategories().Where(x => x.Domain == DomainName);
        }

        public IQueryable<Tag> GetAllTags()
        {
            return Service.GetAllTags().Where(x => x.Domain == DomainName);
        }

        public async Task<Item?> GetItem(string? id)
        {
            var result = await Service.GetItem(id);
            if (result is not null && result.Domain == DomainName)
                return result;
            return null;
        }

        public async Task<Category?> GetCategoryByName(string? name)
        {
            var result = await GetAllCategories().Where(x => x.Name == name).FirstOrDefaultAsync();
            if (result is not null)
                return result;
            return null;
        }

        public async Task<Category?> GetCategory(string? id)
        {
            var result = await Service.GetCategory(id);
            if (result is not null && result.Domain == DomainName)
                return result;
            return null;
        }

        public async Task<Tag?> GetTag(string? id)
        {
            var result = await Service.GetTag(id);
            if (result is not null && result.Domain == DomainName)
                return result;
            return null;
        }

        public async Task<Tag?> GetTagByName(string? name)
        {
            var result = await GetAllTags().Where(x => x.Name == name).FirstOrDefaultAsync();
            if (result is not null)
                return result;
            return null;
        }

        public async Task<Tag> AddTag(TagMutation value)
        {
            if(GetTagByName(value.Name) is not null)
            {
                throw new System.Exception("The name has existed.");
            }
            return await Service.AddTag(value with
            {
                Domain = DomainName
            });
        }

        public async Task<Tag?> UpdateTag(TagMutation value)
        {
            var entity = await GetTag(value.Id);
            if (entity is not null)
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
            var entity = await GetTag(id);
            if (entity is not null)
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
            var entity = await GetItem(value.Id);
            if (entity is not null)
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
            var entity = await GetItem(id);
            if (entity is not null)
            {
                return await Service.RemoveItem(id);
            }
            return null;
        }

        public async Task<Category> AddCategory(CategoryMutation value)
        {
            if (GetCategoryByName(value.Name) is not null)
            {
                throw new System.Exception("The name has existed.");
            }
            return await Service.AddCategory(value with
            {
                Domain = DomainName
            });
        }

        public async Task<Category?> UpdateCategory(CategoryMutation value)
        {
            var entity = await GetCategory(value.Id);
            if (entity is not null)
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
            var entity = await GetCategory(id);
            if (entity is not null)
            {
                return await Service.RemoveCategory(id);
            }
            return null;
        }
    }
}
