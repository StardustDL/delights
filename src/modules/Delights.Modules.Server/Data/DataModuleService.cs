using Delights.Modules.Server.Data.Models;
using Delights.Modules.Server.Data.Models.Actions;
using Microsoft.EntityFrameworkCore;
using StardustDL.AspNet.ItemMetadataServer;
using StardustDL.AspNet.ItemMetadataServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data
{
    public abstract class DataModuleService<TDb, TRaw, T, TMutation, TDomain> : IDataModuleService<TRaw, T, TMutation> where TDb : DbContext where TRaw : RawDataItemBase where T : DataItemBase where TMutation : DataMutationItemBase
    {
        public DataModuleService(TDb dbContext, ItemMetadataDomain<TDomain> metadataDomain)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TRaw>();
            MetadataDomain = metadataDomain;
        }

        public TDb DbContext { get; }

        private DbSet<TRaw> DbSet { get; }

        public ItemMetadataDomain<TDomain> MetadataDomain { get; }

        public async Task<T> AddData(TMutation value)
        {
            var metadataMutation = value.Metadata ?? new StardustDL.AspNet.ItemMetadataServer.Models.Actions.ItemMetadataMutation();
            var metadata = await MetadataDomain.AddMetadata(metadataMutation);

            var tag = await CreateByMutation(value);
            tag.Id = value.Id ?? Guid.NewGuid().ToString();
            tag.MetadataId = metadata.Id;

            DbSet.Add(tag);
            await DbContext.SaveChangesAsync();

            await ReloadRaw(tag);
            return await ToData(tag, metadata);
        }

        public async Task<T?> GetData(string? id)
        {
            var result = await DbSet.FindAsync(id);
            if (result is not null)
            {
                await ReloadRaw(result);
                return await ToData(result);
            }
            return null;
        }

        public async Task<T?> GetDataByMetadataID(string? id)
        {
            var result = await DbSet.Where(x => x.MetadataId == id).FirstOrDefaultAsync();
            if (result is not null)
            {
                await ReloadRaw(result);
                return await ToData(result);
            }
            return null;
        }

        public virtual async Task Initialize()
        {
            await DbContext.Database.EnsureCreatedAsync();
            await DbContext.SaveChangesAsync();
        }

        public virtual IQueryable<TRaw> QueryAllRawData()
        {
            return DbSet;
        }

        public virtual async Task<T?> RemoveData(string id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity is not null)
            {
                await ReloadRaw(entity);

                var metadata = await MetadataDomain.RemoveMetadata(entity.MetadataId!);
                var result = await ToData(entity, metadata);
                DbSet.Remove(entity);
                await DbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<T?> UpdateData(TMutation value)
        {
            var tag = await DbSet.FindAsync(value.Id);
            if (tag is not null)
            {
                await ApplyMutation(tag, value);

                ItemMetadata? metadata = null;
                if (value.Metadata is not null)
                {
                    metadata = await MetadataDomain.UpdateMetadata(value.Metadata with
                    {
                        Id = tag.MetadataId
                    });
                }

                await DbContext.SaveChangesAsync();

                await ReloadRaw(tag);
                return await ToData(tag, metadata);
            }
            return null;
        }

        protected virtual async Task ReloadRaw(TRaw value)
        {
            var entry = DbContext.Entry(value);
            await entry.ReloadAsync();
        }

        protected virtual async Task<T> ToData(TRaw raw, ItemMetadata? metadata = null)
        {
            if (metadata is null)
            {
                metadata = (await MetadataDomain.GetItem(raw.MetadataId))?.AsMetadata();
                if (metadata is null)
                    metadata = new ItemMetadata();
            }
            return await RawToData(raw) with
            {
                Id = raw.Id ?? "",
                Metadata = metadata
            };
        }

        protected abstract Task<T> RawToData(TRaw raw);

        protected abstract Task ApplyMutation(TRaw raw, TMutation mutation);

        protected abstract Task<TRaw> CreateByMutation(TMutation mutation);
    }
}
