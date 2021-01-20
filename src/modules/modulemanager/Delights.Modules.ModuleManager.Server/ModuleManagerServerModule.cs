using Modulight.Modules.Server.GraphQL;
using System;
using System.Collections.Generic;
using Modulight.Modules;

namespace Delights.Modules.ModuleManager.Server
{
    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class ModuleManagerServerModule : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public ModuleManagerServerModule() : base()
        {
        }
    }
}
