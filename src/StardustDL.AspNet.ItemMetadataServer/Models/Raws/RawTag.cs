using System;
using System.Collections.Generic;

namespace StardustDL.AspNet.ItemMetadataServer.Models.Raws
{
    public class RawTag
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public string Name { get; set; } = "";

        public ICollection<RawItem>? Items { get; set; }
    }
}
