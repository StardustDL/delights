using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules
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
        /// Entry assembly name
        /// </summary>
        public string EntryAssembly { get; init; } = "";

        /// <summary>
        /// Additional assemblies
        /// </summary>
        public string[] Assemblies { get; init; } = Array.Empty<string>();

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

        /// <summary>
        /// Generate manifest from a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ModuleManifest Generate(Type type)
        {
            // Split by upper chars
            static string[] GenerateName(string typeName)
            {
                if (typeName.EndsWith("Module"))
                    typeName = typeName[0..^6];
                List<int> splitIndexs;
                {
                    SortedSet<int> indexSet = new SortedSet<int>(new[] { 0, typeName.Length });
                    foreach (int i in Enumerable.Range(0, typeName.Length))
                    {
                        if (char.IsUpper(typeName[i]))
                            indexSet.Add(i);
                    }
                    splitIndexs = indexSet.ToList();
                }
                List<string> names = new List<string>();
                foreach (int i in Enumerable.Range(0, splitIndexs.Count - 1))
                {
                    names.Add(typeName[splitIndexs[i]..splitIndexs[i + 1]]);
                }
                return names.ToArray();
            }

            var attribute = type.GetCustomAttribute<ModuleAttribute>(true);

            var result = new ModuleManifest
            {
                Name = attribute?.Name ?? string.Concat(GenerateName(type.Name)),
                EntryAssembly = attribute?.EntryAssembly ?? type.GetAssemblyName(),
                DisplayName = attribute?.DisplayName ?? string.Join(' ', GenerateName(type.Name)),
                Version = attribute?.Version ?? type.Assembly.GetName().Version?.ToString() ?? "0.0.0.0",
                Author = attribute?.Author ?? "Anonymous",
                Assemblies = attribute?.Assemblies ?? Array.Empty<string>(),
                Description = attribute?.Description ?? "",
                Url = attribute?.Url ?? "",
            };
            return result;
        }
    }

    /// <summary>
    /// Set module manifest.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ModuleAttribute : Attribute
    {
        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Entry assembly name
        /// </summary>
        public string? EntryAssembly { get; init; }

        /// <summary>
        /// Additional assemblies
        /// </summary>
        public string[]? Assemblies { get; init; }

        /// <summary>
        /// Display name
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Version
        /// </summary>
        public string? Version { get; init; }

        /// <summary>
        /// Author
        /// </summary>
        public string? Author { get; init; }

        /// <summary>
        /// Project URL
        /// </summary>
        public string? Url { get; init; }
    }
}
