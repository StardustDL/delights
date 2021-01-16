using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Extension methods for razor component modules.
    /// </summary>
    public static class RazorComponentClientModuleExtensions
    {
        /// <summary>
        /// Use building middlewares for razor component modules.
        /// It will register <see cref="IRazorComponentClientModuleHost"/> service.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UseRazorComponentClientModules(this IModuleHostBuilder modules)
        {
            return modules.UsePostMiddleware((modules, services) =>
            {
                services.TryAddScoped<LazyAssemblyLoader>();
                services.AddSingleton<IRazorComponentClientModuleHost>(sp => new RazorComponentClientModuleHost(sp,
                    modules.Modules.AllSpecifyModules<IRazorComponentClientModule>().ToArray()));
            });
        }

        /// <summary>
        /// Get razor component module host from service provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IRazorComponentClientModuleHost GetRazorComponentClientModuleHost(this IServiceProvider provider) => provider.GetRequiredService<IRazorComponentClientModuleHost>();
    }
}
