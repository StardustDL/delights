using Delights.Modules.Notes.Server.Data;
using Delights.Modules.Notes.Server.Models;
using Delights.Modules.Notes.Server.Models.Actions;
using Delights.Modules.Server.Data;
using Microsoft.Extensions.Logging;
using StardustDL.AspNet.ItemMetadataServer;
using System;
using System.Threading.Tasks;

namespace Delights.Modules.Notes.Server
{
    public class ModuleService : DataModuleService<DataDbContext, RawNote, Note, NoteMutation, NotesServerModule>
    {
        public ModuleService(IServiceProvider services, DataDbContext dbContext, IItemMetadataDomain<NotesServerModule> metadataDomain, ILogger<NotesServerModule> logger) : base(dbContext, metadataDomain)
        {
            Services = services;
            Logger = logger;
        }

        ILogger<NotesServerModule> Logger { get; set; }

        IServiceProvider Services { get; }

        protected override Task ApplyMutation(RawNote raw, NoteMutation mutation)
        {
            if (mutation.Title is not null)
                raw.Title = mutation.Title;
            if (mutation.Content is not null)
                raw.Content = mutation.Content;
            return Task.CompletedTask;
        }

        protected override Task<Note> RawToData(RawNote raw)
        {
            return Task.FromResult(new Note
            {
                Content = raw.Content,
                Title = raw.Title,
            });
        }

        protected override Task<NoteMutation> DataToMutation(Note data)
        {
            return Task.FromResult(new NoteMutation
            {
                Content = data.Content,
                Title = data.Title,
                Id = data.Id,
                Metadata = data.Metadata.AsMutation(),
            });
        }
    }
}
