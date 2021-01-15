using Minio;
using System.Threading;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ObjectStorage
{
    public class BucketService
    {
        internal BucketService(string name, MinioClient client)
        {
            Client = client;
            Name = name;
        }

        MinioClient Client { get; }

        public string Name { get; }

        public Task<bool> Exists(CancellationToken cancellationToken = default) => Client.BucketExistsAsync(Name, cancellationToken: cancellationToken);

        public Task Make(CancellationToken cancellationToken = default) => Client.MakeBucketAsync(Name, cancellationToken: cancellationToken);

        public async Task Remove(CancellationToken cancellationToken = default)
        {
            // NOTE: not remove objects
            await Client.RemoveBucketAsync(Name, cancellationToken: cancellationToken);
        }

        public ObjectService Object(string name)
        {
            return new ObjectService(Name, name, Client);
        }
    }
}
