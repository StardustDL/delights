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
using Modulight.Modules.Hosting;

namespace Delights.Modules.Client
{

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = "Provide components for Delights client modules.")]
    //TODO: dep antd, vditor, mat icon
    public class ClientModule : Module<ClientModule>
    {
        public ClientModule(IModuleHost host) : base(host)
        {
        }
    }
}
