using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Client.UI
{
    public class PageTitle : ComponentBase
    {
        [CascadingParameter]
        protected PageParent Parent { get; set; }

        [Parameter]
        public string Value { get; set; } = "";

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Parent.Title = Value;
        }
    }
}
