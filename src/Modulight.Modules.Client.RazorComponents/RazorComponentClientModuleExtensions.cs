using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Extension methods for razor component modules.
    /// </summary>
    public static class RazorComponentClientModuleExtensions
    {
        /// <summary>
        /// Use building middlewares for razor component modules.
        /// It will register <see cref="IRazorComponentClientModuleCollection"/> service.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        [Obsolete("No need to use this method. The plugin has been removed.")]
        public static IModuleHostBuilder UseRazorComponentClientModules(this IModuleHostBuilder modules)
        {
            return modules;
        }

        /// <summary>
        /// Get razor component module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleCollection GetRazorComponentClientModuleCollection(this IServiceProvider provider) => provider.GetRequiredService<IRazorComponentClientModuleCollection>();
    }
}
