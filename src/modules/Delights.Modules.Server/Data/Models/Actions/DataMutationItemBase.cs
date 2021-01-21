using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data.Models.Actions
{
    public abstract record DataMutationItemBase
    {
        public string? Id { get; init; }

        public ItemMetadataMutation? Metadata { get; init; }
    }
}
