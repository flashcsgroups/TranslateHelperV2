using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Web;
using Server.Web.Models;

namespace TranslateHelper.Controllers
{
    [Produces("application/json")]
    public class IdiomCategoryController : Controller
    {
        [Route("api/idiomcategory")]
        [HttpGet]
        public IActionResult Get(int[] Id)
        {
            List<IdiomCategory> items = new List<IdiomCategory>();
            using (var db = new ServerDbContext())
            {
                items = db.IdiomCategory.Where(item=>Id.Contains(item.IdiomCategoryID)).ToList();
            }
            return new ObjectResult(items);
        }

        [Route("api/idiomcategory/changes")]
        [HttpGet]
        public IActionResult Changes(DateTime clientMaxDate)
        {
            List<IdiomCategory> items = new List<IdiomCategory>();
            using (var db = new ServerDbContext())
            {
                items = db.IdiomCategory.Where(item => item.UpdateDate > clientMaxDate).ToList();
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
