using Microsoft.AspNetCore.Components;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Container for module pages.
    /// Provide page services to interop with host UI.
    /// </summary>
    public abstract class PageParentBase : ComponentBase
    {
        private string _title = "";
        private IRazorComponentClientModule? _module;

        /// <summary>
        /// Get or set page title.
        /// </summary>
        public string Title
        {
            get => _title; set
            {
                if (_title != value)
                {
                    _title = value;
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Get or set current module.
        /// </summary>
        public IRazorComponentClientModule? Module
        {
            get => _module; set
            {
                if (_module != value)
                {
                    _module = value;
                    StateHasChanged();
                }
            }
        }
    }
}
