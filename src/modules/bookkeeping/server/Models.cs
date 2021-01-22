using Delights.Modules.Server.Data.Models;
using Delights.Modules.Server.Data.Models.Actions;
using StardustDL.AspNet.ItemMetadataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Bookkeeping.Server.Models
{
    public enum AmountUnit
    {
        CNY,
    }

    public record AccountItem : DataItemBase
    {
        public string Title { get; init; } = "";

        public AccountAmount Amount { get; init; } = new AccountAmount();
    }

    public record AccountAmount
    {
        public double Value { get; init; } = 0;

        public AmountUnit Unit { get; init; }


        public Actions.AccountAmountMutation AsMutation()
        {
            return new Actions.AccountAmountMutation
            {
                Unit = Unit,
                Value = Value,
            };
        }
    }

    public class RawAccountItem : RawDataItemBase
    {
        public string Title { get; set; } = "";

        public double AmountValue { get; set; } = 0;

        public AmountUnit AmountUnit { get; set; }
    }
}

namespace Delights.Modules.Bookkeeping.Server.Models.Actions
{
    public record AccountItemMutation : DataMutationItemBase
    {
        public string? Title { get; init; }

        public AccountAmountMutation? Amount { get; init; }
    }

    public record AccountAmountMutation
    {
        public double? Value { get; init; }

        public AmountUnit? Unit { get; init; }
    }
}