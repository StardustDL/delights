using System;
using System.Collections.Generic;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public class Tag
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public string Name { get; set; } = "";

        public ICollection<Item>? Items { get; set; }
    }
}
