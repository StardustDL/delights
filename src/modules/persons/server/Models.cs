using Delights.Modules.Server.Data.Models;
using Delights.Modules.Server.Data.Models.Actions;

namespace Delights.Modules.Persons.Server.Models
{
    public enum PersonGender
    {
        Unknown,
        Male,
        Female,
    }

    public record Person : DataItemBase
    {
        public string Name { get; init; } = "";

        public PersonGender Gender { get; init; }

        public string Avatar { get; init; } = "";

        public string Profile { get; init; } = "";
    }

    public class RawPerson : RawDataItemBase
    {
        public string Name { get; set; } = "";

        public PersonGender Gender { get; set; }

        public string Avatar { get; set; } = "";

        public string Profile { get; set; } = "";
    }
}

namespace Delights.Modules.Persons.Server.Models.Actions
{
    public record PersonMutation : DataMutationItemBase
    {
        public string? Name { get; init; }

        public PersonGender? Gender { get; init; }

        public string? Avatar { get; init; }

        public string? Profile { get; init; }
    }
}