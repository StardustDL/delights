using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleUIRootPathAttribute : Attribute
    {
        public ModuleUIRootPathAttribute(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; init; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleUIResourceAttribute : Attribute
    {
        public ModuleUIResourceAttribute(UIResourceType type, string path)
        {
            Type = type;
            Path = path;
        }

        /// <summary>
        /// Resource type.
        /// </summary>
        public UIResourceType Type { get; init; }

        /// <summary>
        /// Resource relative path.
        /// </summary>
        public string Path { get; init; } = string.Empty;

        /// <summary>
        /// Attributes for the resource.
        /// </summary>
        public string[] Attributes { get; init; } = Array.Empty<string>();
    }

    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    /// <summary>
    /// Basic implement for <see cref="IModuleUI"/>.
    /// </summary>
    public abstract class ModuleUI : IDisposable, IAsyncDisposable, IModuleUI
    {
        Dictionary<string, Lazy<Task<IJSObjectReference>>> JSInvokers { get; } = new Dictionary<string, Lazy<Task<IJSObjectReference>>>();

        /// <summary>
        /// Create a module UI instance.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="logger"></param>
        public ModuleUI(IJSRuntime jsRuntime, ILogger logger)
        {
            RootPath = "";
            JSRuntime = jsRuntime;
            Logger = logger;

            var type = GetType();
            {
                var attr = type.GetCustomAttribute<ModuleUIRootPathAttribute>();
                if (attr is not null)
                    RootPath = attr.RootPath;
            }
            {
                var attrs = type.GetCustomAttributes<ModuleUIResourceAttribute>();
                List<UIResource> resources = new List<UIResource>();
                foreach (var attr in attrs)
                {
                    resources.Add(new UIResource(attr.Type, attr.Path) { Attributes = attr.Attributes });
                }
                Resources = resources.ToArray();
            }
        }

        /// <inheritdoc/>

        public virtual RenderFragment? Icon => null;

        /// <inheritdoc/>
        public string RootPath { get; }

        /// <inheritdoc/>
        public virtual bool Contains(string path)
        {
            if (RootPath is "")
            {
                return true;
            }
            path = path.Trim('/') + "/";
            return path.StartsWith($"{RootPath}/");
        }

        /// <inheritdoc/>
        public UIResource[] Resources { get; protected set; }

        /// <summary>
        /// JS runtime.
        /// </summary>
        protected IJSRuntime JSRuntime { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Get a lazy javascript module at /_content/{<paramref name="assemblyName"/>}/{<paramref name="jsPath"/>}.
        /// </summary>
        /// <param name="jsPath">Javascript file path.</param>
        /// <param name="assemblyName">Assembly name, null for the assembly which current module defined.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get default javascript module with default name "module.js".
        /// </summary>
        /// <returns></returns>
        protected virtual Task<IJSObjectReference> GetEntryJSModule() => GetJSModule("module.js");

        #region Dispose

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        private bool _disposedValue;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
