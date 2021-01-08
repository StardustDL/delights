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

        protected Task<IJSObjectReference> GetJSModule() => GetJSInvoker("moduleCore.js");

        public async ValueTask CacheDataFromPath(string path, bool forceUpdate = false)
        {
            var js = await GetJSModule();
            await js.InvokeVoidAsync("cacheDataFromPath", path, forceUpdate);
        }

        public async ValueTask LoadScript(string src)
        {
            var js = await GetJSModule();
            await js.InvokeVoidAsync("loadScript", src, );
        }

        public async ValueTask LoadStyleSheet(string href)
        {
            var js = await GetJSModule();
            await js.InvokeVoidAsync("loadStyleSheet", href);
        }
    }

    public class CoreModuleService : ModuleService
    {

    }
}
