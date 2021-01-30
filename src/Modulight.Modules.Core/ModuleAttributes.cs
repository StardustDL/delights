using Microsoft.Extensions.DependencyInjection;
using System;

namespace Modulight.Modules
{
    /// <summary>
    /// Set module manifest.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleAttribute : Attribute
    {
        /// <summary>
        /// Set module manifest.
        /// </summary>
        public ModuleAttribute() : this(null) { }

        /// <summary>
        /// Set module manifest.
        /// </summary>
        public ModuleAttribute(string? name = null)
        {
            Name = name;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get; init; }

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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ModuleAssemblyAttribute : Attribute
    {
        public ModuleAssemblyAttribute(string assembly)
        {
            Assembly = assembly;
        }

        /// <summary>
        /// Additional assemblies
        /// </summary>
        public string Assembly { get; init; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleServiceAttribute : Attribute
    {
        public ModuleServiceAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; init; }

        public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Scoped;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleOptionAttribute : Attribute
    {
        public ModuleOptionAttribute(Type optionType)
        {
            OptionType = optionType;
        }

        public Type OptionType { get; init; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleStartupAttribute : Attribute
    {
        public ModuleStartupAttribute(Type startupType)
        {
            StartupType = startupType;
        }

        public Type StartupType { get; init; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ModuleDependencyAttribute : Attribute
    {
        public ModuleDependencyAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }

        public Type ModuleType { get; init; }
    }
}
