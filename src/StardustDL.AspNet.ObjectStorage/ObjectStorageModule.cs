using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Modulight.Modules;
using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Services;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StardustDL.AspNet.ObjectStorage
{
    public class ObjectStorageModule : Module<ObjectStorageService, ObjectStorageModuleOption>
    {
        public ObjectStorageModule() : base()
        {
            Manifest = Manifest with
            {
                Name = "ObjectStorageProvider",
                DisplayName = "Object Storage Provider",
                Description = "Provide object storage services.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }
    }

    public class ObjectStorageService : IModuleService
    {
        public ObjectStorageService(IOptions<ObjectStorageModuleOption> options)
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

        public ObjectStorageModuleOption Options { get; }

        MinioClient Client { get; }

        public BucketService Bucket(string bucketName)
        {
            return new BucketService(bucketName, Client);
        }
    }

    public class ObjectStorageModuleOption
    {
        public string Endpoint { get; set; } = "";

        public string AccessKey { get; set; } = "";

        public string SecretKey { get; set; } = "";

        public bool Ssl { get; set; } = false;
    }
}
