using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modulight.Modules;
using Modulight.Modules.Services;
using StardustDL.AspNet.ItemMetadataProvider.Data;
using StardustDL.AspNet.ItemMetadataProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer
{
    [Module(Description = "Provide Item Metadata Server services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    public class ItemMetadataServerModule : Module<ItemMetadataServerService, ItemMetadataServerModuleOption>
    {
        public ItemMetadataServerModule() : base()
        {
        }

        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);

            var options = GetSetupOptions(new ItemMetadataServerModuleOption());

            services.AddPooledDbContextFactory<Data.ItemMetadataDbContext>(o =>
            {
                if (options.ConfigureDbContext is not null)
                    options.ConfigureDbContext(o);
            });
        }
    }

    public class ItemMetadataServerService : IModuleService
    {
        public ItemMetadataServerService(IServiceProvider services, IOptions<ItemMetadataServerModuleOption> options)
        {
            Services = services;
            Options = options.Value;
        }

        public IServiceProvider Services { get; }

        public ItemMetadataServerModuleOption Options { get; }
    }

    public class ItemMetadataServerModuleOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }
}
