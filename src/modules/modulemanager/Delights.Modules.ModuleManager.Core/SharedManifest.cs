using Modulight.Modules;
using System;

namespace Delights.Modules.ModuleManager
{
    public static class SharedManifest
    {
        public static ModuleManifest Raw => new ModuleManifest
        {
            Name = "ModuleManager",
            DisplayName = "Module Manager",
            Description = "Manage client modules and server modules.",
            Url = "https://github.com/StardustDL/delights",
            Author = "StardustDL",
        };
    }
}
