﻿using System;

namespace StardustDL.AspNet.ItemMetadataServer.Models.Actions
{
    public record ItemMetadataMutation
    {
        public string? Id { get; init; }

        public DateTimeOffset? CreationTime { get; init; }

        public DateTimeOffset? ModificationTime { get; init; }

        public DateTimeOffset? AccessTime { get; init; }

        public string? Remarks { get; init; }

        public string? Category { get; init; }

        public string[]? Tags { get; init; }

        public string? Attachments { get; set; }
    }
}
