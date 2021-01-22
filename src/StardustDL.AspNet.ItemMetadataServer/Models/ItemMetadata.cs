using StardustDL.AspNet.ItemMetadataServer.Models.Actions;
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

        public ItemMetadataMutation AsMutation()
        {
            return new ItemMetadataMutation
            {
                Id = Id,
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
                AccessTime = AccessTime,
                Remarks = Remarks,
                Attachments = Attachments,
                Category = Category,
                Tags = Tags,
            };
        }
    }
}
