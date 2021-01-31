#nullable disable

using Microsoft.EntityFrameworkCore;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;

namespace StardustDL.AspNet.ItemMetadataServer.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(
            DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<RawItemMetadata> Items { get; set; }

        public DbSet<RawCategory> Categories { get; set; }

        public DbSet<RawTag> Tags { get; set; }
    }
}
