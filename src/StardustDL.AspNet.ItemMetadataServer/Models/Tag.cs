using System.Collections.Generic;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public class Tag
    {
        public string Id { get; set; } = "";

        public Domain? Domain { get; set; }

        public string DomainId { get; set; } = "";

        public string Name { get; set; } = "";

        public ICollection<ItemMetadata>? Items { get; set; }
    }
}
