using System;
using System.Collections.Generic;

namespace StardustDL.AspNet.ItemMetadataServer.Models.Raws
{
    public class RawCategory
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public string Name { get; set; } = "";

        public ICollection<RawItemMetadata>? Items { get; set; }
    }
}
