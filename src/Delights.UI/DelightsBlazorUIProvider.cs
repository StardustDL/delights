using Modulight.Modules.Client.RazorComponents;
using Modulight.UI.Blazor.Services;
using System;

namespace Delights.UI
{
    public class DelightsBlazorUIProvider : BlazorUIProvider
    {
        public DelightsBlazorUIProvider(IRazorComponentClientModuleCollection razorComponentClientModuleCollection) : base(razorComponentClientModuleCollection)
        {
            SiteInfo = SiteInfo with
            {
                Name = "Delights",
                Onwer = "StardustDL",
                StartTime = DateTimeOffset.MinValue.AddYears(2019),
            };
        }
    }
}
