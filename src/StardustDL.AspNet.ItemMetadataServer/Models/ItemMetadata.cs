using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ItemMetadataServer.Models
{
    public class ItemMetadata
    {
        public string? Id { get; set; }

        public string Domain { get; set; } = "";

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public DateTimeOffset AccessTime { get; set; }

        public string Remarks { get; set; } = "";

        public Category? Category { get; set; }

        public ICollection<Tag>? Tags { get; set; }
    }
}
