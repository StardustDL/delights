#nullable disable

using Delights.Modules.Persons.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Delights.Modules.Persons.Server.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(
            DbContextOptions<DataDbContext> options) : base(options)
        {
        }

        public DbSet<RawPerson> Persons { get; set; }
    }
}
