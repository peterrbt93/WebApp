using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Net.Http.Headers;
using System.Net;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "value1", "value2", "value3"
        };

        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        //Default is as JSON
        [HttpGet("GetValues")]
        //public IEnumerable<Values> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new Values
        //    {
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        public IActionResult Get()
        {
            var retVal = new { key1 = "value1", key2 = "value2" };
            return Ok(retVal);
        }

        //If you want XML you can do this and set custom error code
        [HttpGet("{value}")]
        [Produces("application/xml")]
        public IActionResult GetXml(string value)
        {
            string xml = $"<result><value>{value}</value></result>";

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = xml,
                StatusCode = 200
            };
        }
    }
}