using System.Linq;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data
{
    public interface IDataModuleService<TRaw, T, TMutation>
    {
        Task<DumpedData<T>> Dump();

        Task<bool> LoadDump(DumpedData<T> dumpedData);

        IQueryable<TRaw> QueryAllRawData();

        Task<T?> GetData(string? id);

        Task<T?> GetDataByMetadataID(string? id);

        Task<T> AddData(TMutation value);

        Task<T?> UpdateData(TMutation value);

        Task<T?> RemoveData(string id);
    }
}
