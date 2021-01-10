using Delights.Modules.Client.RazorComponents.UI;
using Delights.Modules.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.Extensions.DependencyInjection;
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
    public class Module : ClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = "CoreClient",
                DisplayName = "Core Client",
                Description = "Provide core functions for client modules.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }

        public override void RegisterService(IServiceCollection services)
        {
            base.RegisterService(services);
            services.AddScoped<LazyAssemblyLoader>();
        }

        public override async Task Initialize(IServiceProvider provider)
        {
            await base.Initialize(provider);

            if (Environment.OSVersion.Platform == PlatformID.Other)
            {
                var modules = provider.GetModuleHost();
                var clientui = GetUI(provider);
                foreach (var module in modules.AllSpecifyModules<IClientModule>())
                {
                    var ui = module.GetUI(provider);
                    foreach (var resource in ui.Resources)
                    {
                        switch (resource.Type)
                        {
                            case UIResourceType.Script:
                                await clientui.LoadScript(resource.Path);
                                break;
                            case UIResourceType.StyleSheet:
                                await clientui.LoadStyleSheet(resource.Path);
                                break;
                        }
                    }
                }
            }

            var option = GetOption(provider);
            if (option.Validation)
            {
                HashSet<string> rootPaths = new HashSet<string>();
                var modules = provider.GetModuleHost();
                var clientService = GetService(provider);
                foreach (var module in modules.AllSpecifyModules<IClientModule>())
                {
                    var ui = module.GetUI(provider);
                    if (rootPaths.Contains(ui.RootPath))
                    {
                        throw new Exception($"Same RootPath in modules: {ui.RootPath} @ {module.Manifest.Name}");
                    }
                    rootPaths.Add(ui.RootPath);

                    await clientService.GetAssembliesForRouting($"/{ui.RootPath}");
                }
            }
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
        public ModuleService(IModuleHost modules, IServiceProvider serviceProvider, LazyAssemblyLoader lazyAssemblyLoader, ILogger<Module> logger)
        {
            Modules = modules;
            ServiceProvider = serviceProvider;
            LazyAssemblyLoader = lazyAssemblyLoader;
            Logger = logger;
        }

        public IModuleHost Modules { get; }

        public IServiceProvider ServiceProvider { get; }

        public LazyAssemblyLoader LazyAssemblyLoader { get; }

        public ILogger<Module> Logger { get; }

        public async Task<List<Assembly>> GetAssembliesForRouting(string path, CancellationToken cancellationToken = default)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Assembly> results = new List<Assembly>();

            Queue<string> toLoad = new Queue<string>();

            foreach (var module in Modules.AllSpecifyModules<IClientModule>())
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
