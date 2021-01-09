using System;

namespace Delights.Modules.Hello
{
    public static class SharedMetadata
    {
        public static ModuleMetadata Raw => new ModuleMetadata
        {
            Name = "Hello",
            DisplayName = "Hello",
            Description = "A hello module.",
        };
    }
}
