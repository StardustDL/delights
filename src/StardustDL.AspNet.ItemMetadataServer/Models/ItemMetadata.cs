using System;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public record ItemMetadata
    {
        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }

        public DateTimeOffset AccessTime { get; init; }

        public string Id { get; set; } = "";

        public string Remarks { get; init; } = "";

        public string Attachments { get; init; } = "";

        public string Category { get; init; } = "";

        public string[] Tags { get; init; } = Array.Empty<string>();
    }
}
