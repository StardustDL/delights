#nullable disable

using Delights.Modules.Bookkeeping.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Delights.Modules.Bookkeeping.Server.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(
            DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<RawAccountItem> Bookkeeping { get; set; }
    }
}
