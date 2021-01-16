using Modulight.Modules.Server.GraphQL;
using Modulight.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Modulight.Modules;

namespace Delights.Modules.ModuleManager.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddModuleManagerModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<ModuleManagerServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class ModuleManagerServerModule : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public ModuleManagerServerModule() : base()
        {
        }
    }

    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ModuleManifest> GetModules([Service] IModuleHost collection)
        {
            return collection.Modules.Select(m => m.Manifest).AsQueryable();
        }
    }

    public class ModuleService : IModuleService
    {
        public ModuleService(ILogger<ModuleManagerServerModule> logger) => Logger = logger;

        public ILogger<ModuleManagerServerModule> Logger { get; private set; }
    }
}
