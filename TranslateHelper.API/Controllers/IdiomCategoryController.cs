using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslateHelper.Models;

namespace TranslateHelper.Controllers
{
    [Produces("application/json")]
    [Route("api/IdiomCategory")]
    public class IdiomCategoryController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<Models.IdiomCategory> items = new List<Models.IdiomCategory>();
            using (var db = new THdbcontext())
            {
                items = db.IdiomCategory.ToList();
            }
            return new ObjectResult(items);
        }

        // GET: api/IdiomCategory/5
        /*[HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/IdiomCategory
        /*[HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/IdiomCategory/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
