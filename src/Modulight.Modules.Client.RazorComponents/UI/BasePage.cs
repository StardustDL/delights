using Modulight.Modules.Client.RazorComponents.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulight.Modules.Client.RazorComponents.UI
{
    public abstract class BasePage : ComponentBase
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        [CascadingParameter]
        protected PageParent Parent { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public abstract IRazorComponentClientModule GetModule();

        protected override void OnInitialized()
        {
            Parent.Module = GetModule();
            base.OnInitialized();
        }
    }

    public abstract class BasePage<TModule> : BasePage where TModule : IRazorComponentClientModule
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        [Inject]
        public TModule Module { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public override IRazorComponentClientModule GetModule() => Module;
    }
}
