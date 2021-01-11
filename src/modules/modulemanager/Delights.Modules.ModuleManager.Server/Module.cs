using Delights.Modules.Server.GraphQL;
using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delights.Modules.ModuleManager.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddModuleManagerModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.AddModule<Module, ModuleOption>(configureOptions);
            return modules;
        }
    }

    public class Module : GraphQLServerModule<ModuleService, ModuleOption, ModuleQuery, ModuleMutation, ModuleSubscription>
    {
        public Module() : base()
        {
            Manifest = Manifest with
            {
                Name = SharedManifest.Raw.Name,
                DisplayName = SharedManifest.Raw.DisplayName,
                Description = SharedManifest.Raw.Description,
                Url = SharedManifest.Raw.Url,
                Author = SharedManifest.Raw.Author,
            };
        }
    }

    public class ModuleQuery : QueryRootObject
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ModuleManifest> GetModuleManagerModules([Service] IModuleHost collection)
        {
            return collection.Modules.Select(m => m.Manifest).AsQueryable();
        }
    }

    public class ModuleMutation : MutationRootObject
    {
    }

    public class ModuleSubscription : SubscriptionRootObject
    {
    }

    public class ModuleService : Services.IModuleService
    {
        public ModuleService(ILogger<Module> logger) => Logger = logger;

        public ILogger<Module> Logger { get; private set; }
    }
}
