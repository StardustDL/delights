using Microsoft.EntityFrameworkCore;
using System;

namespace StardustDL.AspNet.ItemMetadataServer
{
    public class ItemMetadataServerModuleStartupOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }
}
