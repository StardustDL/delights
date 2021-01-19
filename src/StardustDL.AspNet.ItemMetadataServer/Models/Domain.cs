using System.Collections.Generic;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public class Domain
    {
        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        public ICollection<Category>? Categories { get; set; }

        public ICollection<ItemMetadata>? Items { get; set; }

        public ICollection<Tag>? Tags { get; set; }
    }
}
