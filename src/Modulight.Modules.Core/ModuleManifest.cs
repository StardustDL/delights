using System;

namespace Modulight.Modules
{
    public record ModuleManifest
    {
        public string Name { get; init; } = "";

        public string EntryAssembly { get; init; } = "";

        public string[] Assemblies { get; init; } = Array.Empty<string>();

        public string DisplayName { get; init; } = "";

        public string Description { get; init; } = "";

        public string Version { get; init; } = "";

        public string Author { get; init; } = "";

        public string Url { get; init; } = "";
    }
}
