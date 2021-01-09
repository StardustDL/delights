using Delights.Modules.Client.UI;
using Delights.Modules.Options;
using Delights.Modules.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Client.Core
{
    public class Module : ClientModule<EmptyModuleService<Module>, ModuleOption, ModuleUI>
    {
        public Module() : base()
        {
            Metadata = Metadata with
            {
                Name = "CoreClient",
                DisplayName = "Core Client",
                Description = "Provide core functions for client modules.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }

        public override async Task Initialize(IServiceProvider provider)
        {
            await base.Initialize(provider);

            if (Environment.OSVersion.Platform == PlatformID.Other)
            {
                var modules = provider.GetRequiredService<ModuleCollection>();
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
}
