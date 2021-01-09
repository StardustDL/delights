using Delights.Modules.Server.GraphQL;
using Delights.Modules.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delights.Modules.ModuleManager.Server
{
    public static class ModuleExtensions
    {
        public static ModuleCollection AddModuleManagerModule(this ModuleCollection modules, Action<ModuleOption>? configureOptions = null)
        {
            modules.AddModule<Module, ModuleOption>(configureOptions);
            return modules;
        }
    }

    public class Module : GraphQLServerModule<ModuleService, ModuleOption, ModuleQuery, ModuleMutation, ModuleSubscription>
    {
        public Module() : base()
        {
            Metadata = Metadata with
            {
                Name = SharedMetadata.Raw.Name,
                DisplayName = SharedMetadata.Raw.DisplayName,
                Description = SharedMetadata.Raw.Description,
            };
        }
    }

    public class ModuleQuery : QueryRootObject
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Message> GetModuleManagerMessages([Service] ModuleService service)
        {
            return service.Messages.AsQueryable();
        }
    }

    public class ModuleMutation : MutationRootObject
    {
    }

    public class ModuleSubscription : SubscriptionRootObject
    {
    }

    public record Message
    {
        public string Content { get; init; } = "";
    }

    public class ModuleService : Services.IModuleService
    {
        public List<Message> Messages { get; } = new List<Message>() {
            new Message { Content = "Message 1" },
            new Message { Content = "Message 2" },
        };
    }
}
