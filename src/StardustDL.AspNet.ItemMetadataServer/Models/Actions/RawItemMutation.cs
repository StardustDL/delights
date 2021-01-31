using System;

namespace StardustDL.AspNet.ItemMetadataServer.Models.Actions
{
    public record RawItemMutation
    {
        public string? Id { get; init; }

        public string? Domain { get; init; }

        public DateTimeOffset? CreationTime { get; init; }

        public DateTimeOffset? ModificationTime { get; init; }

        public DateTimeOffset? AccessTime { get; init; }

        public string? Remarks { get; init; }

        public string? CategoryId { get; init; }

        public string[]? TagIds { get; init; }

        public string? Attachments { get; set; }
    }
}
