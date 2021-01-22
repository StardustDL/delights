using Modulight.Modules.Services;
using Microsoft.Extensions.Logging;
using Delights.Modules.Persons.Server.Data;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Delights.Modules.Persons.Server.Models;
using StardustDL.AspNet.ItemMetadataServer;
using StardustDL.AspNet.ItemMetadataServer.Models;
using Delights.Modules.Persons.Server.Models.Actions;
using StardustDL.AspNet.ItemMetadataServer.Models.Raws;
using Delights.Modules.Server.Data;

namespace Delights.Modules.Persons.Server
{
    public class ModuleService : DataModuleService<DataDbContext, RawPerson, Person, PersonMutation, PersonsServerModule>
    {
        public ModuleService(IServiceProvider services, DataDbContext dbContext, ItemMetadataDomain<PersonsServerModule> metadataDomain, IOptions<ModuleOption> options, ILogger<PersonsServerModule> logger) : base(dbContext, metadataDomain)
        {
            Services = services;
            Options = options.Value;
            Logger = logger;
        }

        ILogger<PersonsServerModule> Logger { get; set; }

        IServiceProvider Services { get; }

        ModuleOption Options { get; }

        protected override Task<RawPerson> CreateByMutation(PersonMutation mutation)
        {
            return Task.FromResult(new RawPerson
            {
                Name = mutation.Name ?? "",
                Gender = mutation.Gender ?? PersonGender.Unknown,
            });
        }

        protected override Task ApplyMutation(RawPerson raw, PersonMutation mutation)
        {
            if (mutation.Name is not null)
                raw.Name = mutation.Name;
            if (mutation.Gender is not null)
                raw.Gender = mutation.Gender.Value;
            return Task.CompletedTask;
        }

        protected override Task<Person> RawToData(RawPerson raw)
        {
            return Task.FromResult(new Person
            {
                Gender = raw.Gender,
                Name = raw.Name,
            });
        }
    }
}
