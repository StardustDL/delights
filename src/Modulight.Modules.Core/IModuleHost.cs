using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    public interface IModuleHost
    {
        IList<IModule> Modules { get; }

        Task Initialize();
    }
}