using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Modulight.Modules.Hosting;
using System;
using System.Linq;

namespace Delights.Modules.ModuleManager.Server
{
    /// <summary>
    /// Manifest for module
    /// </summary>
    public record ModuleManifest
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; init; } = "";

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; init; } = "";

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; init; } = "";

        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; init; } = "";

        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; init; } = "";

        /// <summary>
        /// Project URL
        /// </summary>
        public string Url { get; init; } = "";
    }

    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ModuleManifest> GetModules([Service] IModuleHost collection)
        {
            return collection.LoadedModules.Select(m => new ModuleManifest
            {
                Author = m.Manifest.Author,
                Description = m.Manifest.Description,
                DisplayName = m.Manifest.DisplayName,
                Name = m.Manifest.Name,
                Url = m.Manifest.Url,
                Version = m.Manifest.Version,
            }).AsQueryable();
        }
    }
}
