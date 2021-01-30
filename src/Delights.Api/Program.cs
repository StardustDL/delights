using Delights.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using StardustDL.AspNet.IdentityServer;
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

            await host.Services.GetModuleHost().Initialize();

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
                
                {
                    var ims = services.GetRequiredService<Delights.Modules.Notes.Server.ModuleService>();
                    await ims.Initialize();
                }
                {
                    var ims = services.GetRequiredService<Delights.Modules.Persons.Server.ModuleService>();
                    await ims.Initialize();
                }
                {
                    var ims = services.GetRequiredService<Delights.Modules.Bookkeeping.Server.ModuleService>();
                    await ims.Initialize();
                }
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
