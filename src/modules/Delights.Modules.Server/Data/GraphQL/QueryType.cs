using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data.GraphQL
{
    public abstract class QueryType<TDomain, TService, TRaw, T, TMutation> : StardustDL.AspNet.ItemMetadataServer.GraphQL.Clients.QueryType<TDomain> where TService : IDataModuleService<TRaw, T, TMutation>
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public virtual IQueryable<TRaw> GetRawData([Service] TService service)
        {
            return service.QueryAllRawData();
        }

        public virtual async Task<T?> GetDataById(string id, [Service] TService service)
        {
            return await service.GetData(id);
        }

        public virtual async Task<T?> GetDataByMetadataId(string id, [Service] TService service)
        {
            return await service.GetDataByMetadataID(id);
        }

        public virtual async Task<string> GetDump([Service] TService service)
        {
            var result = await service.Dump();
            return await result.DumpToString();
        }
    }
}
