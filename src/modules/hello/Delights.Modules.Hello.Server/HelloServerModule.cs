﻿using Modulight.Modules.Server.GraphQL;
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
        public static IModuleHostBuilder AddHelloModule(this IModuleHostBuilder modules, Action<ModuleOption>? setupOptions = null, Action<ModuleOption, IServiceProvider>? configureOptions = null)
        {
            modules.TryAddModule<HelloServerModule, ModuleOption>(setupOptions, configureOptions);
            return modules;
        }
    }

    [Module(Url = Shared.SharedManifest.Url, Author = Shared.SharedManifest.Author, Description = SharedManifest.Description)]
    public class HelloServerModule : GraphQLServerModule<ModuleService, ModuleOption>
    {
        public override Type QueryType => typeof(ModuleQuery);

        public HelloServerModule() : base()
        {
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
        public ModuleService(ILogger<HelloServerModule> logger) => Logger = logger;

        public ILogger<HelloServerModule> Logger { get; private set; }

        public List<Message> Messages { get; } = new List<Message>() {
            new Message { Content = "Message 1" },
            new Message { Content = "Message 2" },
        };
    }
}