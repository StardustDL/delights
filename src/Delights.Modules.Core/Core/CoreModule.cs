using Delights.Modules.Services;
using Delights.Modules.UI;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Core
{
    public class CoreModule : Module<CoreModuleService, CoreModuleUI>
    {
        public CoreModule() : base("Core")
        {
        }
    }

    public class CoreModuleUI : ModuleUI
    {
        public const string ResourceTagAttrName = "Delights_Module_Resource";

        public CoreModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base("", jsRuntime, logger)
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

    public class CoreModuleService : ModuleService
    {

    }
}
