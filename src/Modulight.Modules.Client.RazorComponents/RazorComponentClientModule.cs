using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules.Client.RazorComponents.UI;
using Modulight.Modules.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Basic implement for <see cref="IRazorComponentClientModule"/>.
    /// </summary>
    public abstract class RazorComponentClientModule<TModule> : Module<TModule>, IRazorComponentClientModule
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        protected RazorComponentClientModule(IModuleHost host) : base(host)
        {
            RootPath = "";
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
    }
}
