using Microsoft.AspNetCore.Components;
using Modulight.Modules.Client.RazorComponents;
using Modulight.UI.Blazor.Components;
using Modulight.UI.Blazor.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Modulight.UI.Blazor.Services
{
    public record SiteInfo
    {
        public string Name { get; init; } = "Site";

        public string Onwer { get; init; } = "Onwer";

        public DateTimeOffset StartTime { get; init; }
    }

    public interface IBlazorUIProvider
    {
        SiteInfo SiteInfo { get; }

        RenderFragment? DefaultModuleIcon { get; }

        RenderFragment? Footer { get; }

        RenderFragment? RouterNotFound { get; }

        RenderFragment? RouterNavigating { get; }

        IEnumerable<IRazorComponentClientModule> GetVisibleClientModules();

        Type DefaultLayout { get; }

        Assembly AppAssembly { get; }
    }

    public class BlazorUIProvider : IBlazorUIProvider
    {
        public BlazorUIProvider(IRazorComponentClientModuleCollection razorComponentClientModuleCollection)
        {
            RazorComponentClientModuleCollection = razorComponentClientModuleCollection;
            SiteInfo = new SiteInfo
            {
                Name = AppAssembly.GetName().Name ?? "Site",
                StartTime = DateTimeOffset.Now
            };
        }

        protected IRazorComponentClientModuleCollection RazorComponentClientModuleCollection { get; }

        public virtual SiteInfo SiteInfo { get; protected set; }

        public virtual RenderFragment? DefaultModuleIcon => Fragments.DefaultModuleIcon;

        public virtual RenderFragment? Footer => Fragments.DefaultFooter(this);

        public virtual RenderFragment? RouterNotFound => Fragments.DefaultRouterNotFound;

        public virtual RenderFragment? RouterNavigating => Fragments.DefaultRouterNavigating;

        public virtual Type DefaultLayout => typeof(ModulePageLayout);

        public virtual Assembly AppAssembly => GetType().Assembly;

        public virtual IEnumerable<IRazorComponentClientModule> GetVisibleClientModules() => RazorComponentClientModuleCollection.LoadedModules.Where(x => x.RootPath is not "");
    }
}
