using Microsoft.AspNetCore.Components;
using System;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Specifies the contract for razor component module UI.
    /// </summary>
    public interface IModuleUI
    {
        /// <summary>
        /// Get module icon.
        /// </summary>
        RenderFragment? Icon { get; }

        /// <summary>
        /// Get module UI resources.
        /// </summary>
        UIResource[] Resources { get; }

        /// <summary>
        /// Get module UI route root path, such as home, search, and so on.
        /// Use <see cref="string.Empty"/> for no page module.
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Check if a path is belongs to this module UI.
        /// </summary>
        /// <param name="path">Route path.</param>
        /// <returns></returns>
        bool Contains(string path);
    }
}