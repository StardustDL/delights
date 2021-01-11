using Modulight.Modules;
using System;

namespace Delights.Modules.Hello
{
    public static class SharedManifest
    {
        public static ModuleManifest Raw => new ModuleManifest
        {
            Name = "Hello",
            DisplayName = "Hello",
            Description = "A hello module.",
            Url = "https://github.com/StardustDL/delights",
            Author = "StardustDL",
        };
    }
}
