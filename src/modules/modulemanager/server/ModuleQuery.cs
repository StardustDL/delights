using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using Modulight.Modules;
using Modulight.Modules.Hosting;

namespace Delights.Modules.ModuleManager.Server
{
    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ModuleManifest> GetModules([Service] IModuleHost collection)
        {
            return collection.LoadedModules.Select(m => m.Manifest).AsQueryable();
        }
    }
}
