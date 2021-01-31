using StardustDL.AspNet.ItemMetadataServer.Models.Actions;

namespace Delights.Modules.Server.Data.Models.Actions
{
    public abstract record DataMutationItemBase
    {
        public string? Id { get; init; }

        public ItemMetadataMutation? Metadata { get; init; }
    }
}
