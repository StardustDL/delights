using Modulight.Modules.Services;
using Microsoft.Extensions.Logging;

namespace Delights.Modules.ModuleManager.Server
{
    public class ModuleService : IModuleService
    {
        public ModuleService(ILogger<ModuleManagerServerModule> logger) => Logger = logger;

        public ILogger<ModuleManagerServerModule> Logger { get; private set; }
    }
}
