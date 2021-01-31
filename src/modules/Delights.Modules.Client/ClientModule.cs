using Modulight.Modules;
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
