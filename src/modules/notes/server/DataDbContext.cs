#nullable disable

using Delights.Modules.Notes.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Delights.Modules.Notes.Server.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(
            DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<RawNote> Notes { get; set; }
    }
}
