using Microsoft.EntityFrameworkCore;
using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public class ItemMetadataDomain<T>
    {
        public const string DefaultCategoryName = "";

        string DomainName => typeof(T).FullName ?? "";

        ModuleService Service { get; }

        public ItemMetadataDomain(ModuleService service)
        {
            Service = service;
        }

        public IQueryable<Item> QueryAllItems()
        {
            return Service.QueryAllItems().Where(x => x.Domain == DomainName);
        }

        public IQueryable<Category> GetAllCategories()
        {
            return Service.QueryAllCategories().Where(x => x.Domain == DomainName);
        }

        public IQueryable<Tag> GetAllTags()
        {
            return Service.QueryAllTags().Where(x => x.Domain == DomainName);
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
            if (GetTagByName(value.Name) is not null)
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

        public async Task<Item> AddItem(ItemMutation value)
        {
            return await Service.AddItem(value with
            {
                Domain = DomainName
            });
        }

        public async Task<Item?> UpdateItem(ItemMutation value)
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

        public async Task<ItemMetadata> AddItemMetadata(ItemMetadataMutation value)
        {
            value = value with
            {
                Category = value.Category ?? DefaultCategoryName
            };
            var mutation = ToItemMutation(value);
            var (category, tags) = await PrepareCategoryAndTag(value.Category, value.Tags);
            var item = await AddItem(mutation with
            {
                CategoryId = category?.Id,
                TagIds = tags?.Select(x => x.Id!).ToArray()
            });
            return item.AsMetadata();
        }

        public async Task<ItemMetadata?> UpdateItemMetadata(ItemMetadataMutation value)
        {
            var entity = await GetItem(value.Id);
            if (entity is not null)
            {
                var (oldCategory, oldTags) = (entity.Category?.Id, entity.Tags?.Select(x => x.Id!).ToArray());
                var mutation = ToItemMutation(value);
                var (category, tags) = await PrepareCategoryAndTag(value.Category, value.Tags);
                var item = await UpdateItem(mutation with
                {
                    CategoryId = category?.Id,
                    TagIds = tags?.Select(x => x.Id!).ToArray()
                });
                await CleanCategoryAndTag(oldCategory, oldTags);
                return item?.AsMetadata();
            }
            return null;
        }

        public async Task<ItemMetadata?> RemoveItemMetadata(string id)
        {
            var entity = await GetItem(id);
            if (entity is not null)
            {
                var (oldCategory, oldTags) = (entity.Category?.Id, entity.Tags?.Select(x => x.Id!).ToArray());
                var item = await RemoveItem(id);
                await CleanCategoryAndTag(oldCategory, oldTags);
                return item?.AsMetadata();
            }
            return null;
        }

        async Task<Category> GetOrCreateCategory(string name)
        {
            var value = await GetCategoryByName(name);
            if (value is null)
            {
                value = await AddCategory(new CategoryMutation
                {
                    Name = name
                });
            }
            return value;
        }

        async Task<Tag> GetOrCreateTag(string name)
        {
            var value = await GetTagByName(name);
            if (value is null)
            {
                value = await AddTag(new TagMutation
                {
                    Name = name
                });
            }
            return value;
        }

        async Task<(Category?, Tag[]?)> PrepareCategoryAndTag(string? categoryName, string[]? tagNames)
        {
            var category = categoryName is null ? null : await GetOrCreateCategory(categoryName);
            List<Tag>? tags = null;
            if (tagNames is not null)
            {
                tags = new List<Tag>();

                foreach (var name in tagNames)
                {
                    tags.Add(await GetOrCreateTag(name));
                }
            }
            return (category, tags?.ToArray());
        }

        async Task CleanCategoryAndTag(string? categoryId, string[]? tagIds)
        {
            if (categoryId is not null)
            {
                var value = await GetCategory(categoryId);
                if (value is not null && value.Items?.Count == 0)
                {
                    await RemoveCategory(categoryId);
                }
            }
            if (tagIds is not null)
            {
                foreach (var id in tagIds)
                {
                    var value = await GetTag(id);
                    if (value is not null && value.Items?.Count == 0)
                    {
                        await RemoveTag(id);
                    }
                }
            }
        }

        ItemMutation ToItemMutation(ItemMetadataMutation mutation)
        {
            return new ItemMutation
            {
                AccessTime = mutation.AccessTime,
                Attachments = mutation.Attachments,
                CreationTime = mutation.CreationTime,
                Domain = DomainName,
                Id = mutation.Id,
                ModificationTime = mutation.ModificationTime,
                Remarks = mutation.Remarks
            };
        }
    }
}
