using Microsoft.EntityFrameworkCore;
using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using System;
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

        public IQueryable<RawItem> QueryAllItems()
        {
            return Service.QueryAllItems().Where(x => x.Domain == DomainName);
        }

        public IQueryable<RawItem> QueryItemsByTag(string name)
        {
            var tag = Service.DbContext.Tags.Where(x => x.Name == name).FirstOrDefault();
            if (tag is null)
            {
                return Array.Empty<RawItem>().AsQueryable();
            }
            else
            {
                return QueryAllItems().Where(x => x.Tags!.Contains(tag) == true);
            }
        }

        public IQueryable<RawItem> QueryItemsByCategory(string name)
        {
            var tag = Service.DbContext.Categories.Where(x => x.Name == name).FirstOrDefault();
            if (tag is null)
            {
                return Array.Empty<RawItem>().AsQueryable();
            }
            else
            {
                return QueryAllItems().Where(x => x.Category == tag);
            }
        }

        public IQueryable<RawCategory> QueryAllCategories()
        {
            return Service.QueryAllCategories().Where(x => x.Domain == DomainName);
        }

        public IQueryable<RawTag> QueryAllTags()
        {
            return Service.QueryAllTags().Where(x => x.Domain == DomainName);
        }

        public async Task<RawItem?> GetItem(string? id)
        {
            var result = await Service.GetItem(id);
            if (result is not null && result.Domain == DomainName)
                return result;
            return null;
        }

        public async Task<RawCategory?> GetCategory(string? name)
        {
            var result = await QueryAllCategories().Where(x => x.Name == name).Select(x => x.Id).FirstOrDefaultAsync();
            if (result is not null)
                return await Service.GetCategory(result);
            return null;
        }

        public async Task<RawTag?> GetTag(string? name)
        {
            var result = await QueryAllTags().Where(x => x.Name == name).Select(x => x.Id).FirstOrDefaultAsync();
            if (result is not null)
                return await Service.GetTag(result);
            return null;
        }

        public async Task<RawTag> AddTag(string name)
        {
            if (await GetTag(name) is not null)
            {
                throw new System.Exception("The name has existed.");
            }
            return await Service.AddTag(new RawTagMutation
            {
                Name = name,
                Domain = DomainName
            });
        }

        public async Task<RawTag?> RenameTag(string oldName, string newName)
        {
            var entity = await GetTag(oldName);
            if (entity is not null)
            {
                return await Service.UpdateTag(new RawTagMutation
                {
                    Id = entity.Id,
                    Name = newName,
                    Domain = DomainName
                });
            }
            return null;
        }

        public async Task<RawTag?> RemoveTag(string name)
        {
            var entity = await GetTag(name);
            if (entity is not null)
            {
                return await Service.RemoveTag(entity.Id!);
            }
            return null;
        }

        public async Task<RawCategory> AddCategory(string name)
        {
            if (await GetCategory(name) is not null)
            {
                throw new System.Exception("The name has existed.");
            }
            return await Service.AddCategory(new RawCategoryMutation
            {
                Name = name,
                Domain = DomainName
            });
        }

        public async Task<RawCategory?> RenameCategory(string oldName, string newName)
        {
            var entity = await GetCategory(oldName);
            if (entity is not null)
            {
                return await Service.UpdateCategory(new RawCategoryMutation
                {
                    Id = entity.Id,
                    Name = newName,
                    Domain = DomainName
                });
            }
            return null;
        }

        public async Task<RawCategory?> RemoveCategory(string name)
        {
            var entity = await GetCategory(name);
            if (entity is not null)
            {
                return await Service.RemoveCategory(entity.Id!);
            }
            return null;
        }

        public async Task<ItemMetadata> AddMetadata(ItemMetadataMutation value)
        {
            value = value with
            {
                Category = value.Category ?? DefaultCategoryName
            };
            var mutation = ToItemMutation(value);
            var (category, tags) = await PrepareCategoryAndTag(value.Category, value.Tags);
            var item = await Service.AddItem(mutation with
            {
                CategoryId = category?.Id,
                TagIds = tags?.Select(x => x.Id!).ToArray(),
            });
            return item.AsMetadata();
        }

        public async Task<ItemMetadata?> UpdateMetadata(ItemMetadataMutation value)
        {
            var entity = await GetItem(value.Id);
            if (entity is not null)
            {
                var (oldCategory, oldTags) = (entity.Category?.Name, entity.Tags?.Select(x => x.Name!).ToArray());
                var mutation = ToItemMutation(value);
                var (category, tags) = await PrepareCategoryAndTag(value.Category, value.Tags);
                var item = await Service.UpdateItem(mutation with
                {
                    CategoryId = category?.Id,
                    TagIds = tags?.Select(x => x.Id!).ToArray(),
                });
                await CleanCategoryAndTag(oldCategory, oldTags);
                return item?.AsMetadata();
            }
            return null;
        }

        public async Task<ItemMetadata?> RemoveMetadata(string id)
        {
            var entity = await GetItem(id);
            if (entity is not null)
            {
                var (oldCategory, oldTags) = (entity.Category?.Id, entity.Tags?.Select(x => x.Id!).ToArray());
                var item = await Service.RemoveItem(id);
                await CleanCategoryAndTag(oldCategory, oldTags);
                return item?.AsMetadata();
            }
            return null;
        }

        async Task<RawCategory> GetOrCreateCategory(string name)
        {
            var value = await GetCategory(name);
            if (value is null)
            {
                value = await AddCategory(name);
            }
            return value;
        }

        async Task<RawTag> GetOrCreateTag(string name)
        {
            var value = await GetTag(name);
            if (value is null)
            {
                value = await AddTag(name);
            }
            return value;
        }

        async Task<(RawCategory?, RawTag[]?)> PrepareCategoryAndTag(string? categoryName, string[]? tagNames)
        {
            var category = categoryName is null ? null : await GetOrCreateCategory(categoryName);
            List<RawTag>? tags = null;
            if (tagNames is not null)
            {
                tags = new List<RawTag>();

                foreach (var name in tagNames)
                {
                    tags.Add(await GetOrCreateTag(name));
                }
            }
            return (category, tags?.ToArray());
        }

        async Task CleanCategoryAndTag(string? categoryName, string[]? tagNames)
        {
            if (categoryName is not null)
            {
                var value = await GetCategory(categoryName);
                if (value is not null && value.Items?.Count == 0)
                {
                    await RemoveCategory(categoryName);
                }
            }
            if (tagNames is not null)
            {
                foreach (var name in tagNames)
                {
                    var value = await GetTag(name);
                    if (value is not null && value.Items?.Count == 0)
                    {
                        await RemoveTag(name);
                    }
                }
            }
        }

        RawItemMutation ToItemMutation(ItemMetadataMutation mutation)
        {
            return new RawItemMutation
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
