using Modulight.Modules.Services;
using Microsoft.Extensions.Logging;
using Delights.Modules.Notes.Server.Data;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Delights.Modules.Notes.Server.Models;
using StardustDL.AspNet.ItemMetadataServer;
using StardustDL.AspNet.ItemMetadataServer.Models;
using Delights.Modules.Notes.Server.Models.Actions;

namespace Delights.Modules.Notes.Server
{
    public class ModuleService : IModuleService
    {
        public ModuleService(IServiceProvider services, DataDbContext dbContext, ItemMetadataDomain<NotesServerModule> metadataDomain, IOptions<ModuleOption> options, ILogger<NotesServerModule> logger)
        {
            Services = services;
            Options = options.Value;
            DbContext = dbContext;
            MetadataDomain = metadataDomain;
            Logger = logger;
        }

        ILogger<NotesServerModule> Logger { get; set; }

        IServiceProvider Services { get; }

        ModuleOption Options { get; }

        internal DataDbContext DbContext { get; }

        ItemMetadataDomain<NotesServerModule> MetadataDomain { get; }

        public async Task Initialize()
        {
            await DbContext.Database.EnsureCreatedAsync();
            await DbContext.Database.MigrateAsync();
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<RawNote> QueryAllNotes()
        {
            return DbContext.Notes;
        }

        public IQueryable<Item> QueryAllMetadata()
        {
            return MetadataDomain.QueryAllItems();
        }

        public async Task<Note?> GetNote(string? id)
        {
            var result = await DbContext.Notes.FindAsync(id);
            if (result is not null)
            {
                await ReloadRawNote(result);
                return await ToNote(result);
            }
            return null;
        }

        public async Task<Note?> GetNoteByMetadataID(string? id)
        {
            var result = await DbContext.Notes.Where(x => x.MetadataId == id).FirstOrDefaultAsync();
            if (result is not null)
            {
                await ReloadRawNote(result);
                return await ToNote(result);
            }
            return null;
        }

        public async Task<Note> AddNote(NoteMutation value)
        {
            var metadataMutation = value.Metadata ?? new StardustDL.AspNet.ItemMetadataServer.Models.Actions.ItemMetadataMutation();
            var metadata = await MetadataDomain.AddItemMetadata(metadataMutation);

            var tag = new RawNote
            {
                Id = value.Id ?? Guid.NewGuid().ToString(),
                Title = value.Title ?? "",
                Content = value.Content ?? "",
                MetadataId = metadata.Id,
            };

            DbContext.Notes.Add(tag);
            await DbContext.SaveChangesAsync();

            await ReloadRawNote(tag);
            return await ToNote(tag, metadata);
        }

        public async Task<Note?> UpdateNote(NoteMutation value)
        {
            var tag = await DbContext.Notes.FindAsync(value.Id);
            if (tag is not null)
            {
                if (value.Title is not null)
                    tag.Title = value.Title;
                if (value.Content is not null)
                    tag.Content = value.Content;
                ItemMetadata? metadata = null;
                if(value.Metadata is not null)
                {
                    metadata = await MetadataDomain.UpdateItemMetadata(value.Metadata with
                    {
                        Id = tag.MetadataId
                    });
                }
                await DbContext.SaveChangesAsync();

                await ReloadRawNote(tag);
                return await ToNote(tag, metadata);
            }
            return null;
        }

        public async Task<Note?> RemoveNote(string id)
        {
            var entity = await DbContext.Notes.FindAsync(id);
            if (entity is not null)
            {
                await ReloadRawNote(entity);

                var metadata = await MetadataDomain.RemoveItemMetadata(entity.MetadataId!);
                var result = await ToNote(entity, metadata);
                DbContext.Notes.Remove(entity);
                await DbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        async Task ReloadRawNote(RawNote value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
        }

        async Task<Note> ToNote(RawNote raw, ItemMetadata? metadata = null)
        {
            if (metadata is null)
            {
                metadata = (await MetadataDomain.GetItem(raw.MetadataId))?.AsMetadata();
                if (metadata is null)
                    metadata = new ItemMetadata();
            }
            return new Note
            {
                Content = raw.Content,
                Title = raw.Title,
                Id = raw.Id ?? "",
                Metadata = metadata
            };
        }
    }
}
