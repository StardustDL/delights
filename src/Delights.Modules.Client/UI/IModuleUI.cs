﻿using Microsoft.AspNetCore.Components;

namespace Delights.Modules.Client.UI
{
    public interface IModuleUI
    {
        RenderFragment? Icon { get; }

        UIResource[] Resources { get; }

        /// <summary>
        /// RootPath, such as home, search, and so on. Empty for no page module.
        /// </summary>
        string RootPath { get; }

        bool Contains(string path);
    }
}