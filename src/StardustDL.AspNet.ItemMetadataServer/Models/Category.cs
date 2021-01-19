using System;
using System.Collections.Generic;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public class Category
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public string Name { get; set; } = "";

        public ICollection<ItemMetadata>? Items { get; set; }
    }
}
