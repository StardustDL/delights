using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    /// <summary>
    /// A razor component to set module page title for <see cref="PageParent"/>.
    /// </summary>
    public class PageInfo : ComponentBase
    {
        /// <summary>
        /// Get <see cref="PageParent"/> instance.
        /// </summary>
        [CascadingParameter]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        protected PageParent Parent { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <summary>
        /// Set current title value.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = "";

        [Parameter]
        public RenderFragment? TitleFragment { get; set; }

        [Parameter]
        public RenderFragment? Content { get; set; }

        [Parameter]
        public RenderFragment? Extra { get; set; }

        [Parameter]
        public RenderFragment? SubtitleFragment { get; set; }

        [Parameter]
        public RenderFragment? Tags { get; set; }

        [Parameter]
        public RenderFragment? Footer { get; set; }

        [Parameter]
        public RenderFragment? Breadcrumb { get; set; }

        /// <inheritdoc/>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent.Title = Title;
            Parent.TitleFragment = TitleFragment;
            Parent.Footer = Footer;
            Parent.Breadcrumb = Breadcrumb;
            Parent.Content = Content;
            Parent.Extra = Extra;
            Parent.Tags = Tags;
            Parent.SubtitleFragment = SubtitleFragment;
        }
    }
}
