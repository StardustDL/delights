using System.Threading;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// Specifies the contract for buckets.
    /// </summary>
    public interface IBucketService
    {
        /// <summary>
        /// Get bucket name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get if the bucket exists.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Exists(CancellationToken cancellationToken = default);

        /// <summary>
        /// Create the bucket.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Make(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a service for the object in the bucket.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IObjectService Object(string name);

        /// <summary>
        /// Remove the bucket.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Remove(CancellationToken cancellationToken = default);
    }
}