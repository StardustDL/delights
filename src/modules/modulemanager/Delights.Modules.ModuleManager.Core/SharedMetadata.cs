using System;

namespace Delights.Modules.ModuleManager
{
    public static class SharedMetadata
    {
        public static ModuleMetadata Raw => new ModuleMetadata
        {
            Name = "ModuleManager",
            DisplayName = "ModuleManager",
            Description = "A modulemanager module.",
        };
    }
}
