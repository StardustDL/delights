using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delights.Modules.Server.Data.Models
{
    public abstract class RawDataItemBase
    {
        public string? Id { get; set; }

        public string? MetadataId { get; set; }
    }
}
