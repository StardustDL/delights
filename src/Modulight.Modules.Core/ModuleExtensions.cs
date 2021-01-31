using System;
using System.Collections.Generic;
using System.Linq;

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

        public static bool IsModule(this Type type) => type.IsModule<IModule>();

        public static bool IsModule<T>(this Type type) where T : IModule => type.IsAssignableTo(typeof(T));

        public static void EnsureModule(this Type type)
        {
            if (!type.IsModule())
                throw new Exception($"{type.FullName} is not a module.");
        }

        public static void EnsureModule<T>(this Type type) where T : IModule
        {
            if (!type.IsModule<T>())
                throw new Exception($"{type.FullName} is not a module typed {typeof(T).FullName}.");
        }

        public static bool IsModuleStartup(this Type type) => type.IsModuleStartup<IModuleStartup>();

        public static bool IsModuleStartup<T>(this Type type) where T : IModuleStartup => type.IsAssignableTo(typeof(T));

        public static void EnsureModuleStartup(this Type type)
        {
            if (!type.IsModuleStartup())
                throw new Exception($"{type.FullName} is not a module startup.");
        }

        public static void EnsureModuleStartup<T>(this Type type) where T : IModuleStartup
        {
            if (!type.IsModuleStartup<T>())
                throw new Exception($"{type.FullName} is not a module startup typed {typeof(T).FullName}.");
        }
    }
}
