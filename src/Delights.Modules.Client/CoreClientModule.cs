using Delights.Modules.Services;
using Delights.Modules.UI;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Client
{
    public class CoreClientModule : ClientModule<ModuleService<CoreClientModule>, CoreClientModuleUI>
    {
        public CoreClientModule() : base("CoreClient")
        {
        }
    }

    public class CoreClientModuleUI : ModuleUI
    {
        public const string ResourceTagAttrName = "Delights_Module_Client_Resource";

        public CoreClientModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger) : base("", jsRuntime, logger)
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
