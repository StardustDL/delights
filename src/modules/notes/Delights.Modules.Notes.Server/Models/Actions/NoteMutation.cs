using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Notes.Server.Models.Actions
{
    public record NoteMutation
    {
        public string? Id { get; init; }

        public ItemMetadataMutation? Metadata { get; init; }

        public string? Title { get; init; }

        public string? Content { get; init; }
    }
}
