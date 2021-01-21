using HotChocolate;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data.GraphQL
{
    public abstract class MutationType<TDomain, TService, TRaw, T, TMutation> : StardustDL.AspNet.ItemMetadataServer.GraphQL.Clients.MutationType<TDomain> where TService : IDataModuleService<TRaw, T, TMutation>
    {
        public virtual async Task<T> CreateData(TMutation mutation, [Service] TService service)
        {
            return await service.AddData(mutation);
        }

        public virtual async Task<T?> DeleteData(string id, [Service] TService service)
        {
            return await service.RemoveData(id);
        }

        public virtual async Task<T?> UpdateData(TMutation mutation, [Service] TService service)
        {
            return await service.UpdateData(mutation);
        }
    }
}
