using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Test.Context;
using System;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class PluginTest
    {
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        class TestPlugin : ModuleHostBuilderPlugin
        {
            public override void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services)
            {
                base.AfterBuild(modules, services);
            }

            public override void AfterModule(Type module, ModuleManifest manifest, IModuleStartup startup, IServiceCollection services)
            {
                base.AfterModule(module, manifest, startup, services);
            }

            public override void BeforeBuild(IModuleHostBuilder builder, IServiceCollection services)
            {
                base.BeforeBuild(builder, services);
            }

            public override void BeforeModule(Type module, ModuleManifest manifest, IModuleStartup startup, IServiceCollection services)
            {
                base.BeforeModule(module, manifest, startup, services);
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var context = new ModuleTestContext<TestModule>().WithPlugin<TestPlugin>();
            await context.Run();
        }
    }
}
