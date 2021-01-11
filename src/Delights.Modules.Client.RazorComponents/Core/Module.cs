﻿using Delights.Modules.Client.RazorComponents.UI;
using Delights.Modules.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Delights.Modules.Client.RazorComponents.Core
{
    public class Module : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = "CoreRazorComponentClient",
                DisplayName = "Core Razor Component Client",
                Description = "Provide core functions for razor component client modules.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            services.TryAddScoped<LazyAssemblyLoader>();
        }
    }

    public class ModuleUI : UI.ModuleUI
    {
        public const string ResourceTagAttrName = "Delights_Module_Client_Resource";

        public ModuleUI(IJSRuntime jsRuntime, ILogger<UI.ModuleUI> logger) : base(jsRuntime, logger)
        {
        }

        public async ValueTask CacheDataFromPath(string path, bool forceUpdate = false)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("cacheDataFromPath", path, forceUpdate);
        }

        public async ValueTask LoadScript(string src)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("loadScript", src, ResourceTagAttrName);
        }

        public async ValueTask LoadStyleSheet(string href)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("loadStyleSheet", href, ResourceTagAttrName);
        }
    }

    public class ModuleService : IModuleService
    {
        public ModuleService(IModuleHost moduleHost, IServiceProvider serviceProvider, IOptions<ModuleOption> options, ModuleUI ui, LazyAssemblyLoader lazyAssemblyLoader, ILogger<Module> logger)
        {
            ModuleHost = moduleHost;
            Options = options.Value;
            UI = ui;
            ServiceProvider = serviceProvider;
            LazyAssemblyLoader = lazyAssemblyLoader;
            Logger = logger;
        }

        public IModuleHost ModuleHost { get; }

        public IServiceProvider ServiceProvider { get; }

        public LazyAssemblyLoader LazyAssemblyLoader { get; }

        public ILogger<Module> Logger { get; }

        public ModuleOption Options { get; }

        public ModuleUI UI { get; }

        public async Task Initialize()
        {
            if (Environment.OSVersion.Platform == PlatformID.Other)
            {
                foreach (var module in ModuleHost.Modules.AllSpecifyModules<IRazorComponentClientModule>())
                {
                    var ui = module.GetUI(ServiceProvider);
                    foreach (var resource in ui.Resources)
                    {
                        switch (resource.Type)
                        {
                            case UIResourceType.Script:
                                await UI.LoadScript(resource.Path);
                                break;
                            case UIResourceType.StyleSheet:
                                await UI.LoadStyleSheet(resource.Path);
                                break;
                        }
                    }
                }
            }

            if (Options.Validation)
            {
                await Validation();
            }
        }

        public async Task Validation()
        {
            HashSet<string> rootPaths = new HashSet<string>();
            foreach (var module in ModuleHost.Modules.AllSpecifyModules<IRazorComponentClientModule>())
            {
                var ui = module.GetUI(ServiceProvider);
                if (rootPaths.Contains(ui.RootPath))
                {
                    throw new Exception($"Same RootPath in modules: {ui.RootPath} @ {module.Manifest.Name}");
                }
                rootPaths.Add(ui.RootPath);

                await GetAssembliesForRouting($"/{ui.RootPath}");
            }
        }

        public async Task<List<Assembly>> GetAssembliesForRouting(string path, CancellationToken cancellationToken = default)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Assembly> results = new List<Assembly>();

            Queue<string> toLoad = new Queue<string>();

            foreach (var module in ModuleHost.Modules.AllSpecifyModules<IRazorComponentClientModule>())
            {
                cancellationToken.ThrowIfCancellationRequested();

                toLoad.Enqueue(module.Manifest.EntryAssembly);

                var ui = module.GetUI(ServiceProvider);

                if (ui.Contains(path))
                {
                    foreach (var name in module.Manifest.Assemblies)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        toLoad.Enqueue(name);
                    }
                }
            }

            while (toLoad.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var current = toLoad.Dequeue();

                Assembly? assembly;

                assembly = loadedAssemblies.FirstOrDefault(x => x.GetName().Name == current);

                if (assembly is null)
                {
                    // Logger.LogInformation($"Loading assembly {current}");
                    if (Environment.OSVersion.Platform == PlatformID.Other)
                    {
                        assembly = (await LazyAssemblyLoader.LoadAssembliesAsync(new[] { current + ".dll" })).FirstOrDefault();
                    }
                    else
                    {
                        assembly = Assembly.Load(current);
                    }
                }

                if (assembly is null)
                {
                    Logger.LogError($"Failed to load assembly {current}");
                    continue;
                }
                else
                {
                    // Logger.LogInformation($"Loaded assembly: {assembly.FullName}");
                }

                results.Add(assembly);

                /*foreach (var refe in assembly.GetReferencedAssemblies())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (refe.Name is not null)
                        toLoad.Enqueue(refe.Name);
                }*/
            }

            return results;
        }
    }
}
