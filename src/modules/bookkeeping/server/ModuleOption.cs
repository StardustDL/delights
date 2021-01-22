using Microsoft.EntityFrameworkCore;
using Modulight.Modules.Server.GraphQL;
using System;

namespace Delights.Modules.Bookkeeping.Server
{
    public class ModuleOption : GraphQLServerModuleOption
    {
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }
    }
}
