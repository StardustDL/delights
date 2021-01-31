using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class DependencyTest
    {
        [ModuleDependency(typeof(TestDepModule))]
        class TestModule : BaseTestModule
        {
            public TestModule(IModuleHost host) : base(host)
            {
            }
        }


        [ModuleDependency(typeof(TestDepDepModule))]
        class TestDepModule : BaseTestModule
        {
            public TestDepModule(IModuleHost host) : base(host)
            {
            }
        }

        class TestDepDepModule : BaseTestModule
        {
            public TestDepDepModule(IModuleHost host) : base(host)
            {
            }
        }

        [TestMethod]
        public async Task Test()
        {
            var context = ModuleTestContext.Create().WithModule<TestModule>();
            context.UseHost(host =>
               {
                   host.Services.GetRequiredService<TestDepDepModule>();
                   host.Services.GetRequiredService<TestDepModule>();
               });
            await context.Run();
        }
    }
}
