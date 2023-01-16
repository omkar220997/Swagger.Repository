using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(CustomConventions))]
    public class ConventionTestController : ControllerBase
    {
        // GET: api/<ConventionTestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ConventionTestController>/5
        [HttpGet("{id}", Name ="Get")]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ConventionTestController>
        [HttpPost]
        //[ApiConventionMethod(typeof(CustomConventions), nameof(CustomConventions.Insert))]
        public void InsertTest([FromBody] string value)
        {
        }

        // PUT api/<ConventionTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ConventionTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
