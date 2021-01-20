using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using Modulight.Modules;
using Delights.Modules.Notes.Server.Models;
using System.Threading.Tasks;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;

namespace Delights.Modules.Notes.Server
{
    public class ModuleQuery : StardustDL.AspNet.ItemMetadataServer.GraphQL.Clients.QueryType<NotesServerModule>
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawNote> GetNotes([Service] ModuleService service)
        {
            return service.QueryAllNotes();
        }

        public async Task<Note?> GetNoteById(string id, [Service] ModuleService service)
        {
            return await service.GetNote(id);
        }

        public async Task<Note?> GetNoteByMetadataId(string id, [Service] ModuleService service)
        {
            return await service.GetNoteByMetadataID(id);
        }
    }
}
