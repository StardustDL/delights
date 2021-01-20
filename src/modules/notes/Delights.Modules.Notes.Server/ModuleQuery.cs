using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using Modulight.Modules;
using Delights.Modules.Notes.Server.Models;
using System.Threading.Tasks;
using StardustDL.AspNet.ItemMetadataServer.Models;

namespace Delights.Modules.Notes.Server
{

    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<RawNote> GetNotes([Service] ModuleService service)
        {
            return service.QueryAllNotes();
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Item> GetMetadata([Service] ModuleService service)
        {
            return service.QueryAllMetadata();
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
