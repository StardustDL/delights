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

namespace Delights.Modules.Hello.Server
{
    public static class ModuleExtensions
    {
        public static IModuleHostBuilder AddHelloModule(this IModuleHostBuilder modules, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<Module, ModuleOption>(configureOptions);
            return modules;
        }
    }

    public class Module : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type? QueryType => typeof(ModuleQuery);

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

    public class ModuleQuery
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Message> GetMessages([Service] ModuleService service)
        {
            service.Logger.LogInformation(nameof(GetMessages));
            return service.Messages.AsQueryable();
        }
    }

    public record Message
    {
        public string Content { get; init; } = "";
    }

    public class ModuleService : IModuleService
    {
        public ModuleService(ILogger<Module> logger) => Logger = logger;

        public ILogger<Module> Logger { get; private set; }

        public List<Message> Messages { get; } = new List<Message>() {
            new Message { Content = "Message 1" },
            new Message { Content = "Message 2" },
        };
    }
}
