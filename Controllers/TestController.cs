using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenGameList.Controllers
{
    [Route("api/[controller]"), EnableCors("AllowLocalhost")]
    public class TestController : Controller
    {
        ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody]JObject value)
        {
            _logger.LogDebug(value.ToString());
            return Ok(new { Id = "ABC123" });
        }

        [HttpGet]
        public IActionResult Get()
        {
            dynamic result = new object[]
            {
                new { Id = "", Name = "Select a Language..." },
                new { Id = "1",Name = "English" },
                new { Id = "2",Name = "German" },
                new { Id = "2",Name = "Spanish" },
                new { Id = "3",Name = "Other" }
            }; 
            return BadRequest("Testing Error");
            // 傳回小寫的屬性名稱（如： id, name）
            // return new JsonResult(result);
            // 不會轉換大小寫
            // return new JsonResult(result, new JsonSerializerSettings {
            //     Formatting = Formatting.Indented
            // });
        }
    }
}
