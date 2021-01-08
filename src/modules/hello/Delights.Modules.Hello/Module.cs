using Delights.Modules.Client;
using Delights.Modules.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Hello
{
    public class Module : ClientModule<ModuleService, ModuleUI>
    {
        public Module() : base("Hello")
        {
            var assemblyName = GetType().Assembly.GetName().Name!;
            Assemblies = new string[]
            {
                assemblyName,
                assemblyName + ".UI",
            };
        }
    }

    public class ModuleUI : Client.UI.ModuleUI
    {
        public ModuleUI(IJSRuntime jsRuntime, ILogger<Client.UI.ModuleUI> logger) : base(jsRuntime, logger, "hello")
        {
        }

        public override RenderFragment Icon => Fragments.Icon;

        public override string DisplayName => "Hello";

        public async ValueTask Prompt(string message)
        {
            var js = await GetEntryJSModule();
            await js.InvokeVoidAsync("showPrompt", message);
        }
    }

    public class ModuleService : Services.ModuleService
    {

    }
}
