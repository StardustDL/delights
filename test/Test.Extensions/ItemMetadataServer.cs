using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulight.Modules.Test;
using Modulight.Modules.Test.Context;
using StardustDL.AspNet.ItemMetadataServer;
using System.Threading.Tasks;

namespace Test.Extensions
{
    [TestClass]
    public class ItemMetadataServer
    {
        [TestMethod]
        public async Task Basic()
        {
            var context = new ModuleTestContext<ItemMetadataServerModule>();
            await context.UseHost(host =>
             {
                 host.HasService<IItemMetadataDomain<ItemMetadataServer>>();
                 host.HasService<IItemMetadataDomain<object>>();
             });
        }
    }
}
