using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    internal class ModuleUILoader : ModuleUI
    {
        public const string ResourceTagAttrName = "Modulight_Module_Client_RazorComponents_Resource";

        public ModuleUILoader(IJSRuntime jsRuntime, ILogger<UI.ModuleUI> logger) : base(jsRuntime, logger)
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
