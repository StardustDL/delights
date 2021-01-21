﻿using Modulight.Modules.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data
{
    public interface IDataModuleService<TRaw, T, TMutation> : IModuleService
    {
        Task Initialize();

        IQueryable<TRaw> QueryAllRawData();

        Task<T?> GetData(string? id);

        Task<T?> GetDataByMetadataID(string? id);

        Task<T> AddData(TMutation value);

        Task<T?> UpdateData(TMutation value);

        Task<T?> RemoveData(string id);
    }
}