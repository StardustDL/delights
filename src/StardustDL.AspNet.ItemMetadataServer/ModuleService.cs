using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules.Services;
using StardustDL.AspNet.ItemMetadataServer.Data;
using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public class ModuleService : IModuleService
    {
        public ModuleService(IServiceProvider services, DataDbContext dbContext, IOptions<ModuleOption> options)
        {
            Services = services;
            Options = options.Value;
            DbContext = dbContext;
        }

        IServiceProvider Services { get; }

        ModuleOption Options { get; }

        internal DataDbContext DbContext { get; }

        public async Task Initialize()
        {
            await DbContext.Database.EnsureCreatedAsync();
            await DbContext.Database.MigrateAsync();
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<Item> QueryAllItems()
        {
            return DbContext.Items;
        }

        public IQueryable<Category> QueryAllCategories()
        {
            return DbContext.Categories;
        }

        public IQueryable<Tag> QueryAllTags()
        {
            return DbContext.Tags;
        }

        public async Task<Item?> GetItem(string? id)
        {
            var result = await DbContext.Items.FindAsync(id);
            if (result is not null)
                await ReloadItem(result);
            return result;
        }

        public async Task<Category?> GetCategory(string? id)
        {
            var result = await DbContext.Categories.FindAsync(id);
            if (result is not null)
                await ReloadCategory(result);
            return result;
        }

        public async Task<Tag?> GetTag(string? id)
        {
            var result = await DbContext.Tags.FindAsync(id);
            if (result is not null)
                await ReloadTag(result);
            return result;
        }

        public async Task<Tag> AddTag(TagMutation value)
        {
            var tag = new Tag
            {
                Id = value.Id ?? Guid.NewGuid().ToString(),
                Name = value.Name ?? "",
                Domain = value.Domain ?? "",
            };
            DbContext.Tags.Add(tag);
            await DbContext.SaveChangesAsync();

            await ReloadTag(tag);
            return tag;
        }

        public async Task<Tag?> UpdateTag(TagMutation value)
        {
            var tag = await GetTag(value.Id);
            if (tag is not null)
            {
                if (value.Domain is not null)
                    tag.Domain = value.Domain;
                if (value.Name is not null)
                    tag.Name = value.Name;
                await DbContext.SaveChangesAsync();

                await ReloadTag(tag);
            }
            return tag;
        }

        public async Task<Tag?> RemoveTag(string id)
        {
            var entity = await GetTag(id);
            if (entity is not null)
            {
                await ReloadTag(entity);

                DbContext.Tags.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<Category> AddCategory(CategoryMutation value)
        {
            var category = new Category
            {
                Id = value.Id ?? Guid.NewGuid().ToString(),
                Name = value.Name ?? "",
                Domain = value.Domain ?? "",
            };
            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();

            await ReloadCategory(category);
            return category;
        }

        public async Task<Category?> UpdateCategory(CategoryMutation value)
        {
            var category = await GetCategory(value.Id);
            if (category is not null)
            {
                if (value.Domain is not null)
                    category.Domain = value.Domain;
                if (value.Name is not null)
                    category.Name = value.Name;
                await DbContext.SaveChangesAsync();

                await ReloadCategory(category);
            }
            return category;
        }

        public async Task<Category?> RemoveCategory(string id)
        {
            var entity = await GetCategory(id);
            if (entity is not null)
            {
                await ReloadCategory(entity);

                DbContext.Categories.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
            return entity;
        }

        public async Task<Item> AddItem(ItemMutation value)
        {
            if (value.CategoryId is null)
            {
                throw new NullReferenceException("Category can't be null");
            }

            var category = DbContext.Categories.Find(value.CategoryId);
            var item = new Item
            {
                Id = value.Id ?? Guid.NewGuid().ToString(),
                AccessTime = value.AccessTime ?? DateTimeOffset.Now,
                ModificationTime = value.ModificationTime ?? DateTimeOffset.Now,
                CreationTime = value.CreationTime ?? DateTimeOffset.Now,
                Domain = value.Domain ?? "",
                Remarks = value.Remarks ?? "",
                Attachments = value.Attachments ?? "",
                Category = category,
            };
            if (value.TagIds is not null)
            {
                item.Tags = value.TagIds.Select(id => DbContext.Tags.Find(id)).ToList();
            }
            DbContext.Items.Add(item);
            await DbContext.SaveChangesAsync();

            await ReloadItem(item);
            return item;
        }

        public async Task<Item?> UpdateItem(ItemMutation value)
        {
            var item = await GetItem(value.Id);
            if (item is not null)
            {
                if (value.Domain is not null)
                    item.Domain = value.Domain;
                if (value.AccessTime is not null)
                    item.AccessTime = value.AccessTime.Value;
                if (value.ModificationTime is not null)
                    item.ModificationTime = value.ModificationTime.Value;
                if (value.CreationTime is not null)
                    item.CreationTime = value.CreationTime.Value;
                if (value.Remarks is not null)
                    item.Remarks = value.Remarks;
                if (value.Attachments is not null)
                    item.Attachments = value.Attachments;
                if (value.CategoryId is not null)
                {
                    var category = DbContext.Categories.Find(value.CategoryId);
                    item.Category = category;
                }
                if (value.TagIds is not null)
                {
                    item.Tags = value.TagIds.Select(id => DbContext.Tags.Find(id)).ToList();
                }
                await DbContext.SaveChangesAsync();

                await ReloadItem(item);
            }
            return item;
        }

        public async Task<Item?> RemoveItem(string id)
        {
            var entity = await GetItem(id);
            if (entity is not null)
            {
                await ReloadItem(entity);

                DbContext.Items.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
            return entity;
        }

        async Task ReloadItem(Item value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
            await entry.Reference(x => x.Category).LoadAsync();
            await entry.Collection(x => x.Tags).LoadAsync();
        }

        async Task ReloadTag(Tag value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
            await entry.Collection(x => x.Items).LoadAsync();
        }

        async Task ReloadCategory(Category value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
            await entry.Collection(x => x.Items).LoadAsync();
        }
    }
}
