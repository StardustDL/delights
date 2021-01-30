using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// A module to provide object storage related services.
    /// </summary>
    [Module(Description = "Provide object storage services.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [ModuleService(typeof(ObjectStorageService))]
    [ModuleOption(typeof(ObjectStorageModuleOption))]
    public class ObjectStorageModule : Module<ObjectStorageModule>
    {
        public ObjectStorageModule(IModuleHost host) : base(host)
        {
        }
    }

    /// <summary>
    /// Services for <see cref="ObjectStorageModule"/>.
    /// </summary>
    public class ObjectStorageService
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        public ObjectStorageService(IOptionsSnapshot<ObjectStorageModuleOption> options)
        {
            Options = options.Value;
            var client = new MinioClient(Options.Endpoint, Options.AccessKey, Options.SecretKey);
            if (Options.Ssl)
            {
                Client = client.WithSSL();
            }
            else
            {
                Client = client;
            }
        }

        ObjectStorageModuleOption Options { get; }

        MinioClient Client { get; }

        /// <summary>
        /// Get a bucket service by name.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public IBucketService Bucket(string bucketName)
        {
            return new BucketService(bucketName, Client);
        }
    }

    /// <summary>
    /// Options for <see cref="ObjectStorageModule"/>.
    /// </summary>
    public class ObjectStorageModuleOption
    {
        /// <summary>
        /// Minio endpoint.
        /// </summary>
        public string Endpoint { get; set; } = "";

        /// <summary>
        /// Minio access key.
        /// </summary>
        public string AccessKey { get; set; } = "";

        /// <summary>
        /// Minio secret key.
        /// </summary>
        public string SecretKey { get; set; } = "";

        /// <summary>
        /// Enable SSL.
        /// </summary>
        public bool Ssl { get; set; } = false;
    }
}
