using Modulight.Modules.Server.AspNet;

namespace Modulight.Modules.Test.Context
{
    public class AspNetServerModuleTestContext : ModuleTestContext
    {
        public AspNetServerModuleTestContext() : base()
        {
            ConfigureBuilder(builder => builder.UseAspNetServerModules());
        }
    }

    public class AspNetServerModuleTestContext<T> : AspNetServerModuleTestContext where T : IAspNetServerModule
    {
        public AspNetServerModuleTestContext() : base()
        {
            WithModule<T>();
        }
    }
}
