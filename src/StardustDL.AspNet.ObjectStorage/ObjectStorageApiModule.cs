using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Modulight.Modules;
using Modulight.Modules.Hosting;
using Modulight.Modules.Server.AspNet;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// A module to provide API controllers <see cref="ObjectStorageController"/> for object storage.
    /// </summary>
    [Module(Description = "Provide API controllers for object storage.", Url = "https://github.com/StardustDL/delights", Author = "StardustDL")]
    [ModuleDependency(typeof(ObjectStorageModule))]
    [ModuleService(typeof(ObjectStorageApiService))]
    [ModuleOption(typeof(ObjectStorageApiModuleOption))]
    [ModuleStartup(typeof(ApiStartup))]
    public class ObjectStorageApiModule : AspNetServerModule<ObjectStorageApiModule>
    {
        /// <summary>
        /// Create the instance.
        /// </summary>
        /// <param name="host"></param>
        public ObjectStorageApiModule(IModuleHost host) : base(host)
        {
        }
    }

    class ApiStartup : ModuleStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().ConfigureApplicationPartManager(apm =>
            {
                apm.ApplicationParts.Add(new AssemblyPart(typeof(ObjectStorageApiModule).Assembly));
            });

            base.ConfigureServices(services);
        }
    }

    /// <summary>
    /// Services for <see cref="ObjectStorageApiModule"/>.
    /// </summary>
    public class ObjectStorageApiService
    {
        /// <summary>
        /// Get the URL in the controller <see cref="ObjectStorageController"/> for a specified object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public string GetObjectUrl(string bucketName, string objectName)
        {
            return $"{ObjectStorageController.RouteName}/{bucketName}/{objectName}";
        }
    }

    /// <summary>
    /// Options for <see cref="ObjectStorageApiModule"/>.
    /// </summary>
    public class ObjectStorageApiModuleOption
    {
        /// <summary>
        /// Enable put methods.
        /// </summary>
        public bool AllowPut { get; set; } = false;

        /// <summary>
        /// Enable get methods.
        /// </summary>
        public bool AllowGet { get; set; } = true;
    }
}
