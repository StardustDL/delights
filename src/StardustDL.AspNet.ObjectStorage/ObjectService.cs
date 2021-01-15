using Minio;
using Minio.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ObjectStorage
{
    public class ObjectService
    {
        public record Info
        {
            public long Size { get; init; }

            public DateTime LastModified { get; init; }

            public string ContentType { get; init; } = "";

            public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
        }

        internal ObjectService(string bucketName, string name, MinioClient client)
        {
            Client = client;
            BucketName = bucketName;
            Name = name;
        }

        MinioClient Client { get; }

        public string BucketName { get; }

        public string Name { get; }

        public async Task<bool> Exists(CancellationToken cancellationToken = default)
        {
            try
            {
                await Client.StatObjectAsync(BucketName, Name, cancellationToken: cancellationToken);
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<Info> Stat(CancellationToken cancellationToken = default)
        {
            var stat = await Client.StatObjectAsync(BucketName, Name, cancellationToken: cancellationToken);
            return new Info
            {
                ContentType = stat.ContentType,
                LastModified = stat.LastModified,
                Metadata = stat.MetaData,
                Size = stat.Size
            };
        }

        public async Task<Stream> Get(CancellationToken cancellationToken = default)
        {
            MemoryStream ms = new MemoryStream();
            await Client.GetObjectAsync(BucketName, Name, stream => stream.CopyTo(ms), cancellationToken: cancellationToken);
            ms.Position = 0;
            return ms;
        }

        public async Task Put(Stream stream, long size, string? contentType = null, CancellationToken cancellationToken = default)
        {
            await Client.PutObjectAsync(BucketName, Name, stream, size, contentType, cancellationToken: cancellationToken);
        }

        public async Task Remove(CancellationToken cancellationToken = default)
        {
            await Client.RemoveObjectAsync(BucketName, Name, cancellationToken: cancellationToken);
        }

        public async Task<string> TemporaryGetUrl(CancellationToken cancellationToken = default)
        {
            return await Client.PresignedGetObjectAsync(BucketName, Name, (int)TimeSpan.FromDays(1).TotalSeconds);
        }

        public async Task<string> TemporaryPutUrl(CancellationToken cancellationToken = default)
        {
            return await Client.PresignedPutObjectAsync(BucketName, Name, (int)TimeSpan.FromDays(1).TotalSeconds);
        }
    }
}
