using Modulight.Modules;
using Modulight.Modules.Hosting;
using StardustDL.RazorComponents.AntDesigns;
using StardustDL.RazorComponents.MaterialDesignIcons;
using StardustDL.RazorComponents.Vditors;

namespace Delights.Modules.Client
{
    /// <summary>
    /// Module for all Delights clients (front-end).
    /// </summary>
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = "Provide components for Delights client modules.")]
    [ModuleDependency(typeof(AntDesignModule))]
    [ModuleDependency(typeof(MaterialDesignIconModule))]
    [ModuleDependency(typeof(VditorModule))]
    public class ClientModule : Module<ClientModule>
    {
        public ClientModule(IModuleHost host) : base(host)
        {
        }
    }
}
