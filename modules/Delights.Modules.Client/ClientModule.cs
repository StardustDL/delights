using Modulight.Modules.Client.RazorComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using StardustDL.RazorComponents.AntDesigns;
using Modulight.Modules;
using StardustDL.RazorComponents.MaterialDesignIcons;
using StardustDL.RazorComponents.Vditors;

namespace Delights.Modules.Client
{

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = "Provide components for Delights client modules.")]
    public class ClientModule : RazorComponentClientModule<ModuleService, ModuleOption, ModuleUI>
    {
        public ClientModule() : base()
        {
        }

        public override void Setup(Modulight.Modules.IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddAntDesignModule();
            host.AddVditorModule();
            host.AddMaterialDesignIconModule();
        }
    }
}
