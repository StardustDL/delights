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
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using Delights.Modules.Server.Data;

namespace Delights.Modules.Notes.Server
{
    public class ModuleService : DataModuleService<DataDbContext, RawNote, Note, NoteMutation, NotesServerModule>
    {
        public ModuleService(IServiceProvider services, DataDbContext dbContext, ItemMetadataDomain<NotesServerModule> metadataDomain, ILogger<NotesServerModule> logger) : base(dbContext, metadataDomain)
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
