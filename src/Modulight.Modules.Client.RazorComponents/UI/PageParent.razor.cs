using Microsoft.AspNetCore.Components;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// Container for module pages.
    /// Provide page services to interop with host UI.
    /// </summary>
    public partial class PageParent : ComponentBase
    {
        private string _title = "";
        private IRazorComponentClientModule? _module;
        private RenderFragment? _titleFragment, _breadcrumb, _subtitleFragment, _tags, _footer, _content, _extra;

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

        public RenderFragment? TitleFragment
        {
            get => _titleFragment; set
            {
                if (_titleFragment != value)
                {
                    _titleFragment = value;
                    StateHasChanged();
                }
            }
        }

        public RenderFragment? SubtitleFragment
        {
            get => _subtitleFragment; set
            {
                if (_subtitleFragment != value)
                {
                    _subtitleFragment = value;
                    StateHasChanged();
                }
            }
        }

        public RenderFragment? Breadcrumb
        {
            get => _breadcrumb; set
            {
                if (_breadcrumb != value)
                {
                    _breadcrumb = value;
                    StateHasChanged();
                }
            }
        }

        public RenderFragment? Tags
        {
            get => _tags; set
            {
                if (_tags != value)
                {
                    _tags = value;
                    StateHasChanged();
                }
            }
        }

        public RenderFragment? Footer
        {
            get => _footer; set
            {
                if (_footer != value)
                {
                    _footer = value;
                    StateHasChanged();
                }
            }
        }

        public RenderFragment? Content
        {
            get => _content; set
            {
                if (_content != value)
                {
                    _content = value;
                    StateHasChanged();
                }
            }
        }

        public RenderFragment? Extra
        {
            get => _extra; set
            {
                if (_extra != value)
                {
                    _extra = value;
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
