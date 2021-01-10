using Microsoft.AspNetCore.Components;

namespace Delights.Modules.Client.RazorComponents.UI
{
    public abstract class PageParentBase : ComponentBase
    {
        private string _title = "";
        private IClientModule? _module;

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

        public IClientModule? Module
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
