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
    [Route("api/Idiom")]
    public class IdiomController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<Models.Idiom> items = new List<Models.Idiom>();
            using (var db = new THdbcontext())
            {
                items = db.Idiom.ToList();
            }
            return new ObjectResult(items);
        }
    }
}