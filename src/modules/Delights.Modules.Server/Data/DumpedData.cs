using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data
{
    public static class DumpedData
    {
        public static async Task<DumpedData<T>?> LoadFromString<T>(string str)
        {
            using var stream = new MemoryStream(Convert.FromBase64String(str));
            return await JsonSerializer.DeserializeAsync<DumpedData<T>>(stream);
        }
    }

    public record DumpedData<T>
    {
        public T[] Data { get; init; } = Array.Empty<T>();

        public string Extra { get; init; } = "";

        public async Task<string> DumpToString()
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, this);
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}
