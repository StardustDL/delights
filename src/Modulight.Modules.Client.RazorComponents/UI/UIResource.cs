using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// UI Resource type.
    /// </summary>
    public enum UIResourceType
    {
        /// <summary>
        /// Javascript
        /// </summary>
        Script,
        /// <summary>
        /// Style sheet
        /// </summary>
        StyleSheet,
    }

    /// <summary>
    /// Defines UI resourcecs.
    /// </summary>
    public record UIResource(UIResourceType Type, string Path)
    {
        /// <summary>
        /// Attributes for the resource.
        /// </summary>
        public string[] Attributes { get; init; } = Array.Empty<string>();
    }
}
