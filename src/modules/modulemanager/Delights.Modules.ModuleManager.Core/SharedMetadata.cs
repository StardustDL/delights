using System;

namespace Delights.Modules.ModuleManager
{
    public static class SharedMetadata
    {
        public static ModuleMetadata Raw => new ModuleMetadata
        {
            Name = "ModuleManager",
            DisplayName = "Module Manager",
            Description = "Manage client modules and server modules.",
        };
    }
}
