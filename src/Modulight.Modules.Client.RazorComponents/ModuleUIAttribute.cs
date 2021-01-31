using System;

namespace Modulight.Modules.Client.RazorComponents
{
    /// <summary>
    /// Specifies the module UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleUIAttribute : Attribute
    {
        /// <summary>
        /// Specifies the module UI.
        /// </summary>
        /// <param name="uiType"></param>
        public ModuleUIAttribute(Type uiType)
        {
            UIType = uiType;
        }

        /// <summary>
        /// Type for the module UI.
        /// </summary>
        public Type UIType { get; init; }
    }
}
