using Delights.Modules.Services;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Delights.Modules
{
    public static class ModuleHostExtensions
    {
        public static T GetModule<T>(this IModuleHost modules)
            where T : class, IModule => (T)modules.Modules.First(m => m.GetType() == typeof(T));

        public static bool TryGetModule<T>(this IModuleHost modules, [NotNullWhen(true)] out T? module)
            where T : class, IModule
        {
            var result = modules.Modules.FirstOrDefault(m => m.GetType() == typeof(T));
            if (result is not null)
            {
                module = (T)result;
                return true;
            }
            else
            {
                module = null;
                return false;
            }
        }

        public static IModuleHost AddModule<T>(this IModuleHost modules)
            where T : class, IModule, new() => modules.AddModule(new T());

        public static IModuleHost AddModule<T, TOption>(this IModuleHost modules, T module, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption>, new() where TOption : class
        {
            modules.AddModule(module);
            if (configureOptions is not null)
            {
                module.ConfigureOptions(configureOptions);
            }
            return modules;
        }

        public static IModuleHost AddModule<T, TOption>(this IModuleHost modules, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption>, new() where TOption : class
        {
            return modules.AddModule(new T(), configureOptions);
        }
    }
}