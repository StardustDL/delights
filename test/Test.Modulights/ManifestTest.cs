﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Modulights
{
    [TestClass]
    public class ManifestTest
    {
        [Module(Author = ManifestString,
        Description = ManifestString,
        DisplayName = ManifestString,
        Name = ManifestString,
        Url = ManifestString,
        Version = ManifestString)]
        [ModuleAssembly(ManifestString)]
        [ModuleAssembly(ManifestString + "1")]
        class TestModule : BaseTestModule
        {
            public const string ManifestString = nameof(TestModule);

            public TestModule(IModuleHost host) : base(host)
            {
            }
        }

        [TestMethod]
        public void Test()
        {
            var context = ModuleTestContext.Create().WithModule<TestModule>();
            context.UseHost(host =>
            {
                var module = host.Services.GetRequiredService<TestModule>();
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Author);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Description);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.DisplayName);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Url);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Version);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Name);
                Assert.AreEqual(TestModule.ManifestString, module.Manifest.Assemblies.FirstOrDefault());
                Assert.AreEqual(TestModule.ManifestString + "1", module.Manifest.Assemblies.Skip(1).FirstOrDefault());
            });
        }
    }
}