using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Services;
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
        /// Get default module host from service provider.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IModuleHost GetModuleHost(this IServiceProvider services)
        {
            return services.GetRequiredService<IModuleHost>();
        }

        /// <summary>
        /// Get assembly name from type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAssemblyName(this Type type) => type.Assembly.GetName().Name!;
    }

    /// <summary>
    /// Extension methods for module hosts.
    /// </summary>
    public static class ModuleHostExtensions
    {
        #region Extension Methods for Module

        /// <summary>
        /// Add a typed module.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <param name="module">Module instance.</param>
        /// <returns></returns>
        public static IModuleHostBuilder AddModule<T>(this IModuleHostBuilder builder, T module) where T : class, IModule => builder.AddModule(typeof(T), module);

        /// <summary>
        /// Add a typed module by empty constructor.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <returns></returns>
        public static IModuleHostBuilder AddModule<T>(this IModuleHostBuilder builder)
            where T : class, IModule, new() => builder.AddModule(new T());

        /// <summary>
        /// Add a typed module with options.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <typeparam name="TOption">Module option type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <param name="module">Module instance.</param>
        /// <param name="setupOptions">Action for <see cref="IModule{TService, TOption}.SetupOptions(Action{TOption})"/>.</param>
        /// <param name="configureOptions">Action for <see cref="IModule{TService, TOption}.ConfigureOptions(Action{TOption, IServiceProvider})"/>.</param>
        /// <returns></returns>
        public static IModuleHostBuilder AddModule<T, TOption>(this IModuleHostBuilder builder, T module, Action<TOption>? setupOptions = null, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption> where TOption : class
        {
            if (setupOptions is not null)
            {
                module.SetupOptions(setupOptions);
            }
            if (configureOptions is not null)
            {
                module.ConfigureOptions(configureOptions);
            }
            builder.AddModule(module);
            return builder;
        }

        /// <summary>
        /// Add a typed module with options by empty constructor.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <typeparam name="TOption">Module option type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <param name="setupOptions">Action for <see cref="IModule{TService, TOption}.SetupOptions(Action{TOption})"/>.</param>
        /// <param name="configureOptions">Action for <see cref="IModule{TService, TOption}.ConfigureOptions(Action{TOption, IServiceProvider})"/>.</param>
        /// <returns></returns>
        public static IModuleHostBuilder AddModule<T, TOption>(this IModuleHostBuilder builder, Action<TOption>? setupOptions = null, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption>, new() where TOption : class => builder.AddModule(new T(), setupOptions, configureOptions);

        /// <summary>
        /// Try to add a module.
        /// </summary>
        /// <param name="type">Module type</param>
        /// <param name="builder">Module host builder.</param>
        /// <param name="module">Function to get module instance.</param>
        /// <returns></returns>
        public static IModuleHostBuilder TryAddModule(this IModuleHostBuilder builder, Type type, Func<IModule> module)
        {
            if (builder.GetModule(type) is null)
            {
                builder.AddModule(type, module());
            }
            return builder;
        }

        /// <summary>
        /// Try to add a typed module.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <param name="module">Function to get module instance.</param>
        /// <returns></returns>
        public static IModuleHostBuilder TryAddModule<T>(this IModuleHostBuilder builder, Func<T> module)
            where T : class, IModule => builder.TryAddModule(typeof(T), () => module());

        /// <summary>
        /// Try to add a typed module by empty constructor.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <returns></returns>
        public static IModuleHostBuilder TryAddModule<T>(this IModuleHostBuilder builder)
            where T : class, IModule, new() => builder.TryAddModule(() => new T());

        /// <summary>
        /// Try to add a typed module with options.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <typeparam name="TOption">Module option type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <param name="module">Function to get module instance.</param>
        /// <param name="setupOptions">Action for <see cref="IModule{TService, TOption}.SetupOptions(Action{TOption})"/>.</param>
        /// <param name="configureOptions">Action for <see cref="IModule{TService, TOption}.ConfigureOptions(Action{TOption, IServiceProvider})"/>.</param>
        /// <returns></returns>
        public static IModuleHostBuilder TryAddModule<T, TOption>(this IModuleHostBuilder builder, Func<T> module, Action<TOption>? setupOptions = null, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption> where TOption : class
        {
            var type = typeof(T);
            if (builder.GetModule(type) is null)
            {
                var mod = module();
                if (setupOptions is not null)
                {
                    mod.SetupOptions(setupOptions);
                }
                if (configureOptions is not null)
                {
                    mod.ConfigureOptions(configureOptions);
                }
                builder.AddModule(type, mod);
            }
            return builder;
        }

        /// <summary>
        /// Try to add a typed module with options by empty constructor.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <typeparam name="TOption">Module option type.</typeparam>
        /// <param name="builder">Module host builder.</param>
        /// <param name="setupOptions">Action for <see cref="IModule{TService, TOption}.SetupOptions(Action{TOption})"/>.</param>
        /// <param name="configureOptions">Action for <see cref="IModule{TService, TOption}.ConfigureOptions(Action{TOption, IServiceProvider})"/>.</param>
        /// <returns></returns>
        public static IModuleHostBuilder TryAddModule<T, TOption>(this IModuleHostBuilder builder, Action<TOption>? setupOptions = null, Action<TOption, IServiceProvider>? configureOptions = null)
            where T : class, IModule<IModuleService, TOption>, new() where TOption : class => builder.TryAddModule(() => new T(), setupOptions, configureOptions);

        #endregion

        /// <summary>
        /// Use a building middleware with <see cref="ModuleHostBuilderMiddlewareType.Pre"/> type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UsePreMiddleware(this IModuleHostBuilder builder, Action<IModuleHostBuilder, IServiceCollection> middleware) => builder.UseMiddleware(ModuleHostBuilderMiddlewareType.Pre, middleware);

        /// <summary>
        /// Use a building middleware with <see cref="ModuleHostBuilderMiddlewareType.Post"/> type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public static IModuleHostBuilder UsePostMiddleware(this IModuleHostBuilder builder, Action<IModuleHostBuilder, IServiceCollection> middleware) => builder.UseMiddleware(ModuleHostBuilderMiddlewareType.Post, middleware);
    }
}
