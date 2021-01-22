#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustDL.AspNet.ItemMetadataServer.Models;
using Delights.Modules.Persons.Server.Models;

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
