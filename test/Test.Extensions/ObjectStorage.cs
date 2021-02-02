using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using StardustDL.AspNet.ObjectStorage;

namespace Test.Extensions
{
    [TestClass]
    public class ObjectStorage
    {
        [TestMethod]
        public void Basic()
        {
            var context = new ModuleTestContext<ObjectStorageModule>();
            context.UseHost(host =>
            {
                // host.HasService<ObjectStorageService>();
            });
        }
    }
}
