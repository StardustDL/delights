using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules.Client.UI
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public abstract class ModuleUI : IAsyncDisposable
    {
        Dictionary<string, Lazy<Task<IJSObjectReference>>> JSInvokers { get; } = new Dictionary<string, Lazy<Task<IJSObjectReference>>>();

        public ModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger, string rootPath = "")
        {
            RootPath = rootPath;
            JSRuntime = jsRuntime;
            Logger = logger;
        }

        public virtual RenderFragment? Icon => null;

        public virtual string DisplayName => "";

        /// <summary>
        /// RootPath, such as home, search, and so on. Empty for no page module.
        /// </summary>
        public string RootPath { get; }

        public UIResource[] Resources { get; protected set; } = Array.Empty<UIResource>();

        public IJSRuntime JSRuntime { get; }

        private ILogger<ModuleUI> Logger { get; }

        protected Task<IJSObjectReference> GetJSInvoker(string jsPath, string? assemblyName = null)
        {
            if (assemblyName is null)
                assemblyName = GetType().Assembly.GetName().Name ?? "";

            string id = $"{assemblyName}/{jsPath}";

            if (!JSInvokers.ContainsKey(id))
            {
                Logger.LogDebug($"Create JS invoker: {id}.");
                JSInvokers.Add(jsPath, new(() =>
                    JSRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{id}").AsTask()));
            }

            return JSInvokers[jsPath].Value;
        }

        protected virtual Task<IJSObjectReference> GetEntryJSModule() => GetJSInvoker("module.js");

        public async ValueTask DisposeAsync()
        {
            foreach (var invoker in JSInvokers)
            {
                if (invoker.Value.IsValueCreated)
                {
                    Logger.LogDebug($"Dispose JS invoker: {invoker.Key}.");
                    var value = await invoker.Value.Value;
                    await value.DisposeAsync();
                }
            }
        }
    }
}
