using Delights.Modules.Server.Data.Models;
using Delights.Modules.Server.Data.Models.Actions;
using StardustDL.AspNet.ItemMetadataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Notes.Server.Models
{
    public record Note : DataItemBase
    {
        public string Title { get; init; } = "";

        public string Content { get; init; } = "";
    }

    public class RawNote : RawDataItemBase
    {
        public string Title { get; set; } = "";

        public string Content { get; set; } = "";
    }
}

namespace Delights.Modules.Notes.Server.Models.Actions
{
    public record NoteMutation : DataMutationItemBase
    {
        public string? Title { get; init; }

        public string? Content { get; init; }
    }
}