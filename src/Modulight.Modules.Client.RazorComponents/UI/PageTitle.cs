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
    public class PageTitle : ComponentBase
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
        public string Value { get; set; } = "";

        /// <inheritdoc/>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent.Title = Value;
        }
    }
}
