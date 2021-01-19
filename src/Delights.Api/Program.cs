using Delights.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modulight.Modules;
using StardustDL.AspNet.IdentityServer;
using StardustDL.AspNet.ItemMetadataServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delights.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                /*await services.GetRequiredService<IdentityServerService>().Initialize(new StardustDL.AspNet.IdentityServer.Models.ApplicationUser
                {
                    UserName = "admin@delights",
                    Email = "admin@delights",
                    EmailConfirmed = true,
                    LockoutEnabled = false
                }, "123P$d");*/
                var ims = services.GetRequiredService<ItemMetadataServerService>();
                await ims.Initialize();
                /*var cg1 = await ims.AddCategory(new StardustDL.AspNet.ItemMetadataServer.Models.Actions.CategoryMutation
                {
                    Domain = "d1",
                    Name = "c1"
                });
                var tg1 = await ims.AddTag(new StardustDL.AspNet.ItemMetadataServer.Models.Actions.TagMutation
                {
                    Domain = "d1",
                    Name = "t1",
                });
                var tg2 = await ims.AddTag(new StardustDL.AspNet.ItemMetadataServer.Models.Actions.TagMutation
                {
                    Domain = "d1",
                    Name = "t2",
                });
                for (int i = 0; i < 10; i++)
                {
                    var it = await ims.AddItem(new StardustDL.AspNet.ItemMetadataServer.Models.Actions.ItemMetadataMutation
                    {
                        Domain = "d1",
                        Remarks = $"item{i}",
                        CategoryId = cg1.Id,
                        TagIds = new string[]
                        {
                            i % 2 == 0 ? tg1.Id : tg2.Id
                        }
                    });
                }*/
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
