namespace StardustDL.AspNet.ItemMetadataServer.Models.Actions
{
    public record RawTagMutation
    {
        public string? Id { get; init; }

        public string? Domain { get; init; }

        public string? Name { get; init; }
    }
}
