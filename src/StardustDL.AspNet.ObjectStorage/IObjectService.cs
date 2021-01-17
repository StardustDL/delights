using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// Specifies the contract for objects.
    /// </summary>
    public interface IObjectService
    {
        /// <summary>
        /// Get the bucket name where the object belongs to.
        /// </summary>
        string BucketName { get; }

        /// <summary>
        /// Get the object name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get if the object exists.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Exists(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a stream to read the object.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> Get(CancellationToken cancellationToken = default);

        /// <summary>
        /// Write a stream to the object.
        /// </summary>
        /// <param name="stream">The data to upload.</param>
        /// <param name="size">Size in bytes.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Put(Stream stream, long size, string? contentType = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove the object.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Remove(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the infomation for the object.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectInfo> Stat(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a temporary URL to read the object.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> TemporaryGetUrl(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a temporary URL to write the object.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> TemporaryPutUrl(CancellationToken cancellationToken = default);
    }
}