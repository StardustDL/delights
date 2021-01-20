using HotChocolate;
using Delights.Modules.Notes.Server.Models;
using System.Threading.Tasks;
using Delights.Modules.Notes.Server.Models.Actions;

namespace Delights.Modules.Notes.Server
{
    public class ModuleMutation
    {
        public async Task<Note> CreateNote(NoteMutation mutation, [Service] Notes.Server.ModuleService service)
        {
            return await service.AddNote(mutation);
        }

        public async Task<Note?> DeleteNote(string id, [Service] Notes.Server.ModuleService service)
        {
            return await service.RemoveNote(id);
        }

        public async Task<Note?> UpdateNote(NoteMutation mutation, [Service] Notes.Server.ModuleService service)
        {
            return await service.UpdateNote(mutation);
        }
    }
}
