using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;
using Modulight.Modules.Hosting;

namespace Delights.Modules.ModuleManager.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    [GraphQLModuleType("ModuleManager", typeof(ModuleQuery))]
    public class ModuleManagerServerModule : GraphQLServerModule<ModuleManagerServerModule>
    {
        public ModuleManagerServerModule(IModuleHost host) : base(host)
        {
        }
    }
}
