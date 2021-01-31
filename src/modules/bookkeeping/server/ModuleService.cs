using Delights.Modules.Bookkeeping.Server.Data;
using Delights.Modules.Bookkeeping.Server.Models;
using Delights.Modules.Bookkeeping.Server.Models.Actions;
using Delights.Modules.Server.Data;
using Microsoft.Extensions.Logging;
using StardustDL.AspNet.ItemMetadataServer;
using System;
using System.Threading.Tasks;

namespace Delights.Modules.Bookkeeping.Server
{
    public class ModuleService : DataModuleService<DataDbContext, RawAccountItem, AccountItem, AccountItemMutation, BookkeepingServerModule>
    {
        public ModuleService(IServiceProvider services, DataDbContext dbContext, ItemMetadataDomain<BookkeepingServerModule> metadataDomain, ILogger<BookkeepingServerModule> logger) : base(dbContext, metadataDomain)
        {
            Services = services;
            Logger = logger;
        }

        ILogger<BookkeepingServerModule> Logger { get; set; }

        IServiceProvider Services { get; }

        protected override Task ApplyMutation(RawAccountItem raw, AccountItemMutation mutation)
        {
            if (mutation.Title is not null)
                raw.Title = mutation.Title;
            if (mutation.Amount is not null)
            {
                if (mutation.Amount.Unit is not null)
                    raw.AmountUnit = mutation.Amount.Unit.Value;
                if (mutation.Amount.Value is not null)
                    raw.AmountValue = mutation.Amount.Value.Value;
            }
            return Task.CompletedTask;
        }

        protected override Task<AccountItem> RawToData(RawAccountItem raw)
        {
            return Task.FromResult(new AccountItem
            {
                Amount = new AccountAmount
                {
                    Unit = raw.AmountUnit,
                    Value = raw.AmountValue,
                },
                Title = raw.Title,
            });
        }

        protected override Task<AccountItemMutation> DataToMutation(AccountItem data)
        {
            return Task.FromResult(new AccountItemMutation
            {
                Amount = data.Amount.AsMutation(),
                Title = data.Title,
                Id = data.Id,
                Metadata = data.Metadata.AsMutation(),
            });
        }
    }
}
