using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDL.AspNet.ItemMetadataServer.Models.Raws
{
    public class RawItemMetadata
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public DateTimeOffset AccessTime { get; set; }

        public string Remarks { get; set; } = "";

        public RawCategory? Category { get; set; }

        public ICollection<RawTag>? Tags { get; set; }

        public string Attachments { get; set; } = "";

        public ItemMetadata AsMetadata()
        {
            return new ItemMetadata
            {
                Id = Id ?? "",
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
                AccessTime = AccessTime,
                Remarks = Remarks,
                Attachments = Attachments,
                Category = Category?.Name ?? "",
                Tags = Tags?.Select(x => x.Name).ToArray() ?? Array.Empty<string>(),
            };
        }
    }
}