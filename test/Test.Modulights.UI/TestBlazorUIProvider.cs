using Modulight.Modules.Client.RazorComponents;
using Modulight.UI.Blazor.Services;

namespace Test.Modulights.UI
{
    internal class TestBlazorUIProvider : BlazorUIProvider
    {
        public TestBlazorUIProvider(IRazorComponentClientModuleCollection razorComponentClientModuleCollection) : base(razorComponentClientModuleCollection)
        {
        }
    }
}
