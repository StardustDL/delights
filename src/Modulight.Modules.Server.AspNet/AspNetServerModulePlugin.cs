using Microsoft.Extensions.DependencyInjection;
using System;
using Modulight.Modules.Hosting;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modulight.Modules.Server.AspNet
{
    internal sealed class AspNetServerModulePlugin : ModuleHostBuilderPlugin
    {
        public override void AfterBuild((Type, ModuleManifest)[] modules, IServiceCollection services)
        {
            services.AddSingleton<IAspNetServerModuleCollection>(sp => new AspNetServerModuleCollection(sp.GetRequiredService<IModuleHost>()));
            base.AfterBuild(modules, services);
        }
    }
}
