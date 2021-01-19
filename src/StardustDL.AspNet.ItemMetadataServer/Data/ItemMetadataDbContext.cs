#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardustDL.AspNet.ItemMetadataServer.Models;

namespace StardustDL.AspNet.ItemMetadataServer.Data
{
    public class ItemMetadataDbContext : DbContext
    {
        public ItemMetadataDbContext(
            DbContextOptions<ItemMetadataDbContext> options) : base(options)
        {
        }

        public DbSet<ItemMetadata> Items { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
