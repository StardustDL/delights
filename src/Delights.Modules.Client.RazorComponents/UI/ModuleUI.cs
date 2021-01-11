using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delights.Modules.Client.RazorComponents.UI
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public abstract class ModuleUI : IDisposable, IAsyncDisposable, IModuleUI
    {
        Dictionary<string, Lazy<Task<IJSObjectReference>>> JSInvokers { get; } = new Dictionary<string, Lazy<Task<IJSObjectReference>>>();

        public ModuleUI(IJSRuntime jsRuntime, ILogger<ModuleUI> logger, string rootPath = "")
        {
            RootPath = rootPath;
            JSRuntime = jsRuntime;
            Logger = logger;
        }

        public virtual RenderFragment? Icon => null;

        /// <summary>
        /// Empty for no page module
        /// </summary>
        public string RootPath { get; }

        public virtual bool Contains(string path)
        {
            if (RootPath is "")
            {
                return true;
            }
            path = path.Trim('/') + "/";
            return path.StartsWith($"{RootPath}/");
        }

        public UIResource[] Resources { get; protected set; } = Array.Empty<UIResource>();

        protected IJSRuntime JSRuntime { get; }

        private ILogger<ModuleUI> Logger { get; }

        protected Task<IJSObjectReference> GetJSModule(string jsPath, string? assemblyName = null)
        {
            if (assemblyName is null)
                assemblyName = GetType().Assembly.GetName().Name ?? "";

            string id = $"{assemblyName}/{jsPath}";

            if (!JSInvokers.ContainsKey(id))
            {
                Logger.LogInformation($"Create JS invoker: {id}.");
                JSInvokers.Add(id, new(() =>
                    JSRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/{id}").AsTask()));
            }

            return JSInvokers[id].Value;
        }

        protected virtual Task<IJSObjectReference> GetEntryJSModule() => GetJSModule("module.js");

        #region Dispose

        protected virtual async ValueTask DisposeAsyncCore()
        {
            foreach (var invoker in JSInvokers)
            {
                if (invoker.Value.IsValueCreated)
                {
                    Logger.LogInformation($"Dispose JS invoker: {invoker.Key}.");
                    var value = await invoker.Value.Value;
                    await value.DisposeAsync();
                }
            }
            JSInvokers.Clear();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var invoker in JSInvokers)
                    {
                        if (invoker.Value.IsValueCreated)
                        {
                            Logger.LogInformation($"Dispose JS invoker: {invoker.Key}.");
                            (invoker.Value.Value as IDisposable)?.Dispose();
                        }
                    }
                    JSInvokers.Clear();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
