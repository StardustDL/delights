using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StardustDL.AspNet.ObjectStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StardustDL.AspNet.ObjectStorage
{
    /// <summary>
    /// Controller for object storage.
    /// </summary>
    [Area(RouteName)]
    [Route("/" + RouteName)]
    [ApiController]
    public class ObjectStorageController : ControllerBase
    {
        internal const string RouteName = nameof(ObjectStorageApiModule);

        /// <summary>
        /// Create the controller.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public ObjectStorageController(ObjectStorageService service, IOptions<ObjectStorageApiModuleOption> options)
        {
            Service = service;
            Options = options.Value;
        }

        ObjectStorageService Service { get; }

        ObjectStorageApiModuleOption Options { get; }

        /// <summary>
        /// Upload a file.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <param name="file"></param>
        /// <returns></returns>

        [HttpPut("{bucketName}/{objectName}")]
        public async Task<ActionResult> Put(string bucketName, string objectName, IFormFile file)
        {
            if (!Options.AllowPut)
                return NoContent();

            using var st = file.OpenReadStream();
            var bucket = Service.Bucket(bucketName);
            var obj = bucket.Object(objectName);
            await obj.Put(st, file.Length, file.ContentType);
            return Ok();
        }

        /// <summary>
        /// Download a file.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        [HttpGet("{bucketName}/{objectName}")]
        public async Task<ActionResult> Get(string bucketName, string objectName)
        {
            if (!Options.AllowGet)
                return NoContent();

            var bucket = Service.Bucket(bucketName);
            var obj = bucket.Object(objectName);
            if (await obj.Exists())
            {
                var stat = await obj.Stat();
                var stream = await obj.Get();
                return File(stream, stat.ContentType);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
