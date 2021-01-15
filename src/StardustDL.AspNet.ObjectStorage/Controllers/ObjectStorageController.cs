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
    [Area(RouteName)]
    [Route("/" + RouteName)]
    [ApiController]
    public class ObjectStorageController : ControllerBase
    {
        internal const string RouteName = nameof(ObjectStorageApiModule);

        public ObjectStorageController(ObjectStorageService service, IOptions<ObjectStorageApiModuleOption> options)
        {
            Service = service;
            Options = options.Value;
        }

        public ObjectStorageService Service { get; }

        public ObjectStorageApiModuleOption Options { get; }

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
