using Delights.Modules.Notes.GraphQL;
using System;

namespace Delights.Modules.Notes
{
    public class UrlGenerator
    {
        public string Notes() => "/notes";

        public string NoteCreate() => "/notes/create";

        public string Note(IData note) => $"/notes/{Uri.EscapeDataString(note.Id)}";

        public string NoteEdit(IData note) => $"/notes/{Uri.EscapeDataString(note.Id)}/edit";

        public string Categories() => $"/notes/categories";

        public string Category(string name) => $"/notes/categories/{Uri.EscapeDataString(name)}";

        public string Tags() => $"/notes/tags";

        public string Tag(string name) => $"/notes/tags/{Uri.EscapeDataString(name)}";
    }
}
