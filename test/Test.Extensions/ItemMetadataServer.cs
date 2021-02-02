using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using StardustDL.AspNet.ItemMetadataServer;

namespace Test.Extensions
{
    [TestClass]
    public class ItemMetadataServer
    {
        [TestMethod]
        public void Basic()
        {
            var context = new ModuleTestContext<ItemMetadataServerModule>();
            context.UseHost(host =>
            {
                host.HasService<IItemMetadataDomain<ItemMetadataServer>>();
                host.HasService<IItemMetadataDomain<object>>();
            });
        }
    }
}
