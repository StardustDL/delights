using Microsoft.EntityFrameworkCore;
using System;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public class ModuleOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }
}
