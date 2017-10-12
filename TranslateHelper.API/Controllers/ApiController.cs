﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslateHelper.Models;

namespace TranslateHelper.Controllers
{
    [Produces("application/json")]
    [Route("Api")]
    public class ApiController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "Description under construction";
        }

        // GET: api/Api/5
        /*[HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }*/

        [HttpGet("api/IdiomCategories")]
        public IActionResult IdiomCategories()
        {
            List<Models.IdiomCategory> items = new List<Models.IdiomCategory>();
            using (var db = new THdbcontext())
            {
                items = db.IdiomCategory.ToList();
            }
            return new ObjectResult(items);
        }
        // POST: api/Api
        /*[HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Api/5
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
