﻿using Modulight.Modules.Client.RazorComponents.UI;
using System;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies the contract for razor component modules.
    /// </summary>
    public interface IRazorComponentClientModule : IModule
    {
        IModuleUI? GetUI(IServiceProvider provider);
    }
}
