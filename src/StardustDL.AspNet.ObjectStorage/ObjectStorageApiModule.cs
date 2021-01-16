using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Modulight.Modules;
using Modulight.Modules.Server.AspNet;
using Modulight.Modules.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StardustDL.AspNet.ObjectStorage
{
    public class ObjectStorageApiModule : AspNetServerModule<ObjectStorageApiService, ObjectStorageApiModuleOption>
    {
        public ObjectStorageApiModule() : base()
        {
            Manifest = Manifest with
            {
                Name = "ObjectStorageApiProvider",
                DisplayName = "Object Storage API Provider",
                Description = $"Provide API controller at /{ObjectStorageController.RouteName} for object storage.",
                Url = "https://github.com/StardustDL/delights",
                Author = "StardustDL",
            };
        }

        public override void RegisterAspNetServices(IServiceCollection services)
        {
            base.RegisterAspNetServices(services);
            services.AddControllers().ConfigureApplicationPartManager(apm =>
            {
                apm.ApplicationParts.Add(new AssemblyPart(typeof(ObjectStorageApiModule).Assembly));
            });
        }

        public override void Setup(IModuleHostBuilder host)
        {
            base.Setup(host);
            host.AddObjectStorageModule();
        }
    }

    public class ObjectStorageApiService : IModuleService
    {
        public string GetObjectUrl(string bucketName, string objectName)
        {
            return $"{ObjectStorageController.RouteName}/{bucketName}/{objectName}";
        }
    }

    public class ObjectStorageApiModuleOption
    {
        public bool AllowPut { get; set; } = false;

        public bool AllowGet { get; set; } = true;
    }
}
