using Microsoft.AspNetCore.Components;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    public interface IModuleUI
    {
        RenderFragment? Icon { get; }

        UIResource[] Resources { get; }

        /// <summary>
        /// RootPath, such as home, search, and so on. Empty for no page module.
        /// </summary>
        string RootPath { get; }

        bool Contains(string path);
    }
}