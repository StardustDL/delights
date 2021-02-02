using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using StardustDL.AspNet.ObjectStorage;
using System.Threading.Tasks;

namespace Test.Extensions
{
    [TestClass]
    public class ObjectStorage
    {
        [TestMethod]
        public async Task Basic()
        {
            var context = new ModuleTestContext<ObjectStorageModule>();
            await context.UseHost(host =>
             {
                 // host.HasService<ObjectStorageService>();
             });
        }
    }
}
