using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Services;
using StardustDL.AspNet.ItemMetadataServer.Data;
using StardustDL.AspNet.ItemMetadataServer.Models;
using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    [Module(Description = "Provide Item Metadata Server services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class ItemMetadataServerModule : Module<ItemMetadataServerService, ItemMetadataServerModuleOption>
    {
        public ItemMetadataServerModule() : base()
        {
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);

            var options = GetSetupOptions(new ItemMetadataServerModuleOption());

            services.AddDbContext<Data.ItemMetadataDbContext>(o =>
            {
                if (options.ConfigureDbContext is not null)
                    options.ConfigureDbContext(o);
            });
        }
    }

    public class ItemMetadataServerService : IModuleService
    {
        public ItemMetadataServerService(IServiceProvider services, ItemMetadataDbContext dbContext, IOptions<ItemMetadataServerModuleOption> options)
        {
            Services = services;
            Options = options.Value;
            DbContext = dbContext;
        }

        IServiceProvider Services { get; }

        ItemMetadataServerModuleOption Options { get; }

        ItemMetadataDbContext DbContext { get; }

        public async Task Initialize()
        {
            await DbContext.Database.EnsureCreatedAsync();
            await DbContext.Database.MigrateAsync();
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<ItemMetadata> GetAllItems()
        {
            return DbContext.Items;
        }

        public IQueryable<Category> GetAllCategories()
        {
            return DbContext.Categories;
        }

        public IQueryable<Tag> GetAllTags()
        {
            return DbContext.Tags;
        }

        public async Task ReloadItem(ItemMetadata value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
            await entry.Reference(x => x.Category).LoadAsync();
            await entry.Collection(x => x.Tags).LoadAsync();
        }

        public async Task ReloadTag(Tag value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
            await entry.Collection(x => x.Items).LoadAsync();
        }

        public async Task ReloadCategory(Category value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
            await entry.Collection(x => x.Items).LoadAsync();
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

        public async Task<Tag> UpdateTag(TagMutation value)
        {
            var tag = DbContext.Tags.Find(value.Id);
            if (value.Domain is not null)
                tag.Domain = value.Domain;
            if (value.Name is not null)
                tag.Name = value.Name;
            await DbContext.SaveChangesAsync();

            await ReloadTag(tag);
            return tag;
        }

        public async Task<Tag> RemoveTag(string id)
        {
            var entity = DbContext.Tags.Find(id);
            await ReloadTag(entity);

            DbContext.Tags.Remove(entity);
            await DbContext.SaveChangesAsync();
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

        public async Task<Category> UpdateCategory(CategoryMutation value)
        {
            var category = DbContext.Categories.Find(value.Id);
            if (value.Domain is not null)
                category.Domain = value.Domain;
            if (value.Name is not null)
                category.Name = value.Name;
            await DbContext.SaveChangesAsync();

            await ReloadCategory(category);
            return category;
        }

        public async Task<Category> RemoveCategory(string id)
        {
            var entity = DbContext.Categories.Find(id);
            await ReloadCategory(entity);

            DbContext.Categories.Remove(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ItemMetadata> AddItem(ItemMetadataMutation value)
        {
            if (value.CategoryId is null)
            {
                throw new NullReferenceException("Category can't be null");
            }

            var category = DbContext.Categories.Find(value.CategoryId);
            var item = new ItemMetadata
            {
                Id = value.Id ?? Guid.NewGuid().ToString(),
                AccessTime = value.AccessTime ?? DateTimeOffset.Now,
                ModificationTime = value.ModificationTime ?? DateTimeOffset.Now,
                CreationTime = value.CreationTime ?? DateTimeOffset.Now,
                Domain = value.Domain ?? "",
                Remarks = value.Remarks ?? "",
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

        public async Task<ItemMetadata> UpdateItem(ItemMetadataMutation value)
        {
            var item = DbContext.Items.Find(value.Id);
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
            return item;
        }

        public async Task<ItemMetadata> RemoveItem(string id)
        {
            var entity = DbContext.Items.Find(id);
            await ReloadItem(entity);

            DbContext.Items.Remove(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
    }

    public class ItemMetadataServerModuleOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }
}
