using Delights.Modules.Persons.Server.Data;
using Delights.Modules.Persons.Server.Models;
using Delights.Modules.Persons.Server.Models.Actions;
using Delights.Modules.Server.Data;
using Microsoft.Extensions.Logging;
using StardustDL.AspNet.ItemMetadataServer;
using System;
using System.Threading.Tasks;

namespace Delights.Modules.Persons.Server
{
    /// <summary>
    /// Service for the <see cref="PersonsServerModule"/>.
    /// </summary>
    public class PersonsServerModuleService : DataModuleService<DataDbContext, RawPerson, Person, PersonMutation, PersonsServerModule>
    {
        public PersonsServerModuleService(IServiceProvider services, DataDbContext dbContext, ItemMetadataDomain<PersonsServerModule> metadataDomain, ILogger<PersonsServerModule> logger) : base(dbContext, metadataDomain)
        {
            Services = services;
            Logger = logger;
        }

        ILogger<PersonsServerModule> Logger { get; set; }

        IServiceProvider Services { get; }

        protected override Task ApplyMutation(RawPerson raw, PersonMutation mutation)
        {
            if (mutation.Name is not null)
                raw.Name = mutation.Name;
            if (mutation.Gender is not null)
                raw.Gender = mutation.Gender.Value;
            if (mutation.Avatar is not null)
                raw.Avatar = mutation.Avatar;
            if (mutation.Profile is not null)
                raw.Profile = mutation.Profile;
            return Task.CompletedTask;
        }

        protected override Task<Person> RawToData(RawPerson raw)
        {
            return Task.FromResult(new Person
            {
                Gender = raw.Gender,
                Name = raw.Name,
                Avatar = raw.Avatar,
                Profile = raw.Profile,
            });
        }

        protected override Task<PersonMutation> DataToMutation(Person data)
        {
            return Task.FromResult(new PersonMutation
            {
                Avatar = data.Avatar,
                Gender = data.Gender,
                Name = data.Name,
                Profile = data.Profile,
                Id = data.Id,
                Metadata = data.Metadata.AsMutation(),
            });
        }
    }
}
