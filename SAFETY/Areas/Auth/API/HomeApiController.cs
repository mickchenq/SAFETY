using SAFETYService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAFETY.Areas.Auth.API
{
    [Area("Auth")]
    [Route("api/[area]/[controller]/[action]")]
    [Authorize]
    public class HomeApiController : Controller
    {
        private readonly CommonService _commonService;

        public HomeApiController(CommonService commonService)
        {
            _commonService = commonService;
        }
        // GET: api/Auth/HomeApi/get
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { _commonService.RandomString(10) };
        }

        // GET api/Auth/HomeApi/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Auth/HomeApi/Post
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/Auth/HomeApi/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/Auth/HomeApi/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
