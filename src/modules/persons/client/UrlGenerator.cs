using Delights.Modules.Persons.GraphQL;
using System;

namespace Delights.Modules.Persons
{
    public class UrlGenerator
    {
        public string Persons() => "/persons";

        public string PersonCreate() => "/persons/create";

        public string Person(IData note) => $"/persons/{Uri.EscapeDataString(note.Id)}";

        public string PersonEdit(IData note) => $"/persons/{Uri.EscapeDataString(note.Id)}/edit";

        public string Categories() => $"/persons/categories";

        public string Category(string name) => $"/persons/categories/{Uri.EscapeDataString(name)}";

        public string Tags() => $"/persons/tags";

        public string Tag(string name) => $"/persons/tags/{Uri.EscapeDataString(name)}";
    }
}
