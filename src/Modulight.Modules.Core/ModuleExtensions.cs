using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Modulight.Modules
{
    /// <summary>
    /// Extension methods for modules.
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Get all modules in given type.
        /// </summary>
        /// <typeparam name="T">Target module type.</typeparam>
        /// <param name="this">Module list.</param>
        /// <returns></returns>
        public static IEnumerable<T> AllSpecifyModules<T>(this IEnumerable<IModule> @this) => @this.Where(m => m is T).Select(m => (T)m);

        /// <summary>
        /// Get assembly name from type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;

        internal static bool IsModule(this Type type) => type.IsAssignableTo(typeof(IModule));

        internal static void EnsureModule(this Type type)
        {
            if (!type.IsModule())
                throw new Exception($"{type.FullName} is not a module.");
        }

        internal static bool IsModuleStartup(this Type type) => type.IsAssignableTo(typeof(IModuleStartup));

        internal static void EnsureModuleStartup(this Type type)
        {
            if (!type.IsModuleStartup())
                throw new Exception($"{type.FullName} is not a module startup.");
        }
    }
}
