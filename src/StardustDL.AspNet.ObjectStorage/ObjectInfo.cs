using System;
using System.Collections.Generic;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// Infomation for objects.
    /// </summary>
    public record ObjectInfo
    {
        /// <summary>
        /// Size in bytes.
        /// </summary>
        public long Size { get; init; }

        /// <summary>
        /// Last modified time.
        /// </summary>
        public DateTime LastModified { get; init; }

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; init; } = "";

        /// <summary>
        /// Extra metadata.
        /// </summary>
        public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
    }
}
