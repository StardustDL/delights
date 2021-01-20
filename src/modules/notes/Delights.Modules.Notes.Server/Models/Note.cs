using StardustDL.AspNet.ItemMetadataServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Notes.Server.Models
{
    public record Note
    {
        public string Id { get; init; } = "";

        public ItemMetadata Metadata { get; init; } = new ItemMetadata();

        public string Title { get; init; } = "";

        public string Content { get; init; } = "";
    }
}
