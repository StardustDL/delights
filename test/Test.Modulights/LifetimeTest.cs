using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class LifetimeTest
    {
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var context = ModuleTestContext.Create().WithModule<TestModule>();
            context.UseHost(host =>
            {
                var module = host.Services.GetRequiredService<TestModule>();
                Assert.IsFalse(module.HasInitialized);
                Assert.IsFalse(module.HasShutdowned);
            });
            await context.Run(host =>
            {
                var module = host.Services.GetRequiredService<TestModule>();
                Assert.IsTrue(module.HasInitialized);
                Assert.IsFalse(module.HasShutdowned);
            }, host =>
            {
                var module = host.Services.GetRequiredService<TestModule>();
                Assert.IsTrue(module.HasInitialized);
                Assert.IsTrue(module.HasShutdowned);
            });
        }
    }

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

            public override void BeforeModule(Type module, ModuleManifest manifest, IServiceCollection services)
            {
                base.BeforeModule(module, manifest, services);
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var context = ModuleTestContext.Create().WithPlugin<TestPlugin>().WithModule<TestModule>();
            await context.Run();
        }
    }
}
