using Modulight.Modules.Client.RazorComponents;

namespace Modulight.Modules.Test.Context
{
    public class RazorComponentClientModuleTestContext : ModuleTestContext
    {
        public RazorComponentClientModuleTestContext() : base()
        {
            ConfigureBuilder(builder => builder.UseRazorComponentClientModules());
        }
    }

    public class RazorComponentClientModuleTestContext<T> : RazorComponentClientModuleTestContext where T : IRazorComponentClientModule
    {
        public RazorComponentClientModuleTestContext() : base()
        {
            WithModule<T>();
        }
    }
}
