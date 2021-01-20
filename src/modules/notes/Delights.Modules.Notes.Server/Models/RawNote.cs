namespace Delights.Modules.Notes.Server.Models
{
    public class RawNote
    {
        public string? Id { get; set; }

        public string? MetadataId { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";
    }
}
