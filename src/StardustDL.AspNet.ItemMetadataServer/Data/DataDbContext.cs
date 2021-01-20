#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;

namespace StardustDL.AspNet.ItemMetadataServer.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(
            DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<RawItem> Items { get; set; }

        public DbSet<RawCategory> Categories { get; set; }

        public DbSet<RawTag> Tags { get; set; }
    }
}
