using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public class Item
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public DateTimeOffset AccessTime { get; set; }

        public string Remarks { get; set; } = "";

        public Category? Category { get; set; }

        public ICollection<Tag>? Tags { get; set; }

        public string Attachments { get; set; } = "";

        public ItemMetadata AsMetadata()
        {
            return new ItemMetadata
            {
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

    public record ItemMetadata
    {
        public DateTimeOffset CreationTime { get; init; }

        public DateTimeOffset ModificationTime { get; init; }

        public DateTimeOffset AccessTime { get; init; }

        public string Remarks { get; init; } = "";

        public string Attachments { get; init; } = "";

        public string Category { get; init; } = "";

        public string[] Tags { get; init; } = Array.Empty<string>();
    }
}
