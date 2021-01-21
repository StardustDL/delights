using StardustDL.AspNet.ItemMetadataServer.Models;

namespace Delights.Modules.Server.Data.Models
{
    public abstract record DataItemBase
    {
        public string Id { get; init; } = "";

        public ItemMetadata Metadata { get; init; } = new ItemMetadata();
    }
}
