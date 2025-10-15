
using Microsoft.AspNetCore.Mvc;
using myapp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myapp.Controllers
{
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly CommonService _commonService;

        public ApiController(CommonService commonService)
        {
            _commonService = commonService;
        }

        // GET: api/{tableName}
        [HttpGet("{tableName}")]
        public async Task<IActionResult> Get(string tableName, [FromQuery] Dictionary<string, string> searchParams)
        {
            var result = await _commonService.GetListAsync(tableName, searchParams);
            return Ok(result);
        }

        // GET: api/{tableName}/{id}
        [HttpGet("{tableName}/{id}")]
        public async Task<IActionResult> GetById(string tableName, string id)
        {
            var result = await _commonService.GetByIdAsync(tableName, id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/{tableName}
        [HttpPost("{tableName}")]
        public async Task<IActionResult> Post(string tableName, [FromBody] Dictionary<string, object> model)
        {
            var result = await _commonService.AddAsync(tableName, model);
            return Ok(new { success = true, id = result });
        }

        // PUT: api/{tableName}/{id}
        [HttpPut("{tableName}/{id}")]
        public async Task<IActionResult> Put(string tableName, string id, [FromBody] Dictionary<string, object> model)
        {
            await _commonService.UpdateAsync(tableName, id, model);
            return Ok(new { success = true });
        }

        // DELETE: api/{tableName}/{id}
        [HttpDelete("{tableName}/{id}")]
        public async Task<IActionResult> Delete(string tableName, string id)
        {
            await _commonService.DeleteAsync(tableName, id);
            return Ok(new { success = true });
        }
    }
}
