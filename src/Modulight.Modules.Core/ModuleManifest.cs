﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules
{
    /// <summary>
    /// Service descriptor for module services.
    /// </summary>
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public record ModuleServiceDescriptor(Type ImplementationType, Type ServiceType, ServiceLifetime Lifetime = ServiceLifetime.Scoped)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
    }

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

        /// <summary>
        /// Services
        /// </summary>
        public ModuleServiceDescriptor[] Services { get; init; } = Array.Empty<ModuleServiceDescriptor>();

        /// <summary>
        /// Options
        /// </summary>
        public Type[] Options { get; init; } = Array.Empty<Type>();

        /// <summary>
        /// Dependencies
        /// </summary>
        public Type[] Dependencies { get; init; } = Array.Empty<Type>();

        /// <summary>
        /// Generate manifest from a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static ModuleManifest Generate(Type type)
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

            var moduleAttr = type.GetCustomAttribute<ModuleAttribute>(true);
            var serviceAttr = type.GetCustomAttributes<ModuleServiceAttribute>(true);
            var optionAttr = type.GetCustomAttributes<ModuleOptionAttribute>(true);
            var depAttr = type.GetCustomAttributes<ModuleDependencyAttribute>(true);

            var deps = new List<Type>();
            foreach (var item in depAttr?.Select(x => x.ModuleType).ToArray() ?? Array.Empty<Type>())
            {
                item.EnsureModule();
                deps.Add(item);
            }

            var result = new ModuleManifest
            {
                Name = moduleAttr?.Name ?? string.Concat(GenerateName(type.Name)),
                DisplayName = moduleAttr?.DisplayName ?? string.Join(' ', GenerateName(type.Name)),
                Version = moduleAttr?.Version ?? type.Assembly.GetName().Version?.ToString() ?? "0.0.0.0",
                Author = moduleAttr?.Author ?? "Anonymous",
                Description = moduleAttr?.Description ?? "",
                Url = moduleAttr?.Url ?? "",
                Services = serviceAttr?.Select(x => new ModuleServiceDescriptor(x.ImplementationType, x.ServiceType ?? x.ImplementationType, x.Lifetime)).ToArray() ?? Array.Empty<ModuleServiceDescriptor>(),
                Options = optionAttr?.Select(x => x.OptionType).ToArray() ?? Array.Empty<Type>(),
                Dependencies = deps.ToArray(),
            };
            return result;
        }
    }
}