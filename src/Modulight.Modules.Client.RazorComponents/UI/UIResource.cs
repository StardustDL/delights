using Microsoft.JSInterop;
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
    public class UIResource
    {
        /// <summary>
        /// Create a UI resource definition.
        /// </summary>
        /// <param name="type">Resource type.</param>
        /// <param name="path">Resource relative path.</param>
        public UIResource(UIResourceType type, string path)
        {
            Type = type;
            Path = path;
        }

        /// <summary>
        /// Resource type.
        /// </summary>
        public UIResourceType Type { get; set; }

        /// <summary>
        /// Resource relative path.
        /// </summary>
        public string Path { get; set; } = string.Empty;
    }
}
