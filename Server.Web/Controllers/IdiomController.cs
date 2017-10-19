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
    public class IdiomController : Controller
    {
        /*[HttpGet]
        public IActionResult Get()
        {
            List<Idiom> items = new List<Idiom>();
            using (var db = new ServerDbContext())
            {
                items = db.Idiom.ToList();
            }
            return new ObjectResult(items);
        }*/
        [Route("api/idiom")]
        [HttpGet]
        public IActionResult Get(int[] Id)
        {
            List<Idiom> items = new List<Idiom>();
            using (var db = new ServerDbContext())
            {
                items = db.Idiom.Where(item=>Id.Contains(item.IdiomID)).ToList();
            }
            return new ObjectResult(items);
        }

        [Route("api/idiom/changes")]
        [HttpGet]
        public IActionResult Changes(DateTime clientMaxDate)
        {
            List<int> items = new List<int>();
            using (var db = new ServerDbContext())
            {
                items = db.Idiom.Where(item => item.UpdateDate>clientMaxDate).Select(item=>item.IdiomID).ToList();
            }
            return new ObjectResult(items);
        }
    }
}