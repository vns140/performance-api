using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.DomainServices.Entities;
using Microservice.DomainServices.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mircroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValuesRepository _valuesRepository;
        public ValuesController(IValuesRepository valuesRepository)
        {
            _valuesRepository = valuesRepository;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] List<Values> values)
        {
            _valuesRepository.AddRangeAsync(values);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices]IConfiguration config, [FromServices]IDistributedCache cache)
        {
            string valorJSON = cache.GetString("Values");
            if (valorJSON == null)
            {
                var result = await _valuesRepository.FindAsync("1dd32938-be8d-411a-aec7-f3ec15aec58a");
                if(result.Count <= 0)
                {
                    return NoContent();
                }
                valorJSON = JsonConvert.SerializeObject(result);
                DistributedCacheEntryOptions opcoesCache =
                    new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(2));

                cache.SetString("Values", valorJSON, opcoesCache);
            }

            return Content(valorJSON, "application/json");
        }        
    }
}
