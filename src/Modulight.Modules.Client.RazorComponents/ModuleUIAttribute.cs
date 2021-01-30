using System;

namespace Modulight.Modules.Client.RazorComponents
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleUIAttribute : Attribute
    {
        public ModuleUIAttribute(Type uiType)
        {
            UIType = uiType;
        }

        public Type UIType { get; init; }
    }
}
