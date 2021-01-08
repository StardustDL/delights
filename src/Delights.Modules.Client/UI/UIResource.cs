using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Delights.Modules.Client.UI
{
    public enum UIResourceType
    {
        Script,
        StyleSheet,
    }

    public class UIResource
    {
        public UIResource(UIResourceType type, string path, bool autoLoad = true, bool cached = true)
        {
            Type = type;
            Path = path;
            AutoLoad = autoLoad;
            AutoCache = cached;
        }

        public UIResourceType Type { get; set; }

        public string Path { get; set; } = string.Empty;

        public bool AutoLoad { get; set; } = true;

        public bool AutoCache { get; set; } = true;
    }
}
