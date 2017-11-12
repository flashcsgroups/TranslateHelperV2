using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Web;
using Server.Web.Models;

namespace Server.Web.Controllers
{
    public class IdiomController : Controller
    {
        private readonly ServerDbContext _context;

        public IdiomController(ServerDbContext context)
        {
            _context = context;
            //_context = new ServerDbContext();
        }

        [Route("api/idiom")]
        [HttpGet]
        public IActionResult Get(int[] Id)
        {
            /*List<Idiom> items = new List<Idiom>();
            using (var db = new ServerDbContext())
            {
                items = db.Idiom.Where(item => Id.Contains(item.IdiomID)).ToList();
            }
            return new ObjectResult(items);*/
            return getIdiomArrayByIds(Id);
        }

        [Route("api/idiom/array")]
        [HttpPost]
        public IActionResult Array([FromBody] int[] Id)
        {
            return getIdiomArrayByIds(Id);
        }

        private IActionResult getIdiomArrayByIds(int[] Id)
        {
            List<Idiom> items = new List<Idiom>();
            using (var db = new ServerDbContext())
            {
                items = db.Idiom.Where(item => Id.Contains(item.IdiomID)).ToList();
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
                items = db.Idiom.Where(item => item.UpdateDate > clientMaxDate).Select(item => item.IdiomID).ToList();
            }
            return new ObjectResult(items);
        }

        // GET: Idiom
        public async Task<IActionResult> Index()
        {
            var serverDbContext = _context.Idiom.Include(i => i.IdiomCategory);
            return View(await serverDbContext.ToListAsync());
        }

        // GET: Idiom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idiom = await _context.Idiom
                .Include(i => i.IdiomCategory)
                .SingleOrDefaultAsync(m => m.IdiomID == id);
            if (idiom == null)
            {
                return NotFound();
            }

            return View(idiom);
        }

        // GET: Idiom/Create
        public IActionResult Create()
        {
            ViewData["IdiomCategoryID"] = new SelectList(_context.IdiomCategory, "IdiomCategoryID", "IdiomCategoryID");
            return View();
        }

        // POST: Idiom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdiomID,IdiomCategoryID,LanguageFrom,LanguageTo,TextFrom,TextTo,ExampleTextFrom,ExampleTextTo,DescriptionTextFrom,DescriptionTextTo")] Idiom idiom)
        {
            if (ModelState.IsValid)
            {
                int maxId = _context.Idiom.Max(item => item.IdiomID);
                idiom.UpdateDate = DateTime.Now;
                idiom.IdiomID = ++maxId;
                _context.Add(idiom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdiomCategoryID"] = new SelectList(_context.IdiomCategory, "IdiomCategoryID", "IdiomCategoryID", idiom.IdiomCategoryID);
            return View(idiom);
        }

        // GET: Idiom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idiom = await _context.Idiom.SingleOrDefaultAsync(m => m.IdiomID == id);
            if (idiom == null)
            {
                return NotFound();
            }
            ViewData["IdiomCategoryID"] = new SelectList(_context.IdiomCategory, "IdiomCategoryID", "IdiomCategoryID", idiom.IdiomCategoryID);
            return View(idiom);
        }

        // POST: Idiom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdiomID,IdiomCategoryID,LanguageFrom,LanguageTo,TextFrom,TextTo,ExampleTextFrom,ExampleTextTo,DescriptionTextFrom,DescriptionTextTo")] Idiom idiom)
        {
            if (id != idiom.IdiomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idiom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdiomExists(idiom.IdiomID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdiomCategoryID"] = new SelectList(_context.IdiomCategory, "IdiomCategoryID", "IdiomCategoryID", idiom.IdiomCategoryID);
            return View(idiom);
        }

        // GET: Idiom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idiom = await _context.Idiom
                .Include(i => i.IdiomCategory)
                .SingleOrDefaultAsync(m => m.IdiomID == id);
            if (idiom == null)
            {
                return NotFound();
            }

            return View(idiom);
        }

        // POST: Idiom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idiom = await _context.Idiom.SingleOrDefaultAsync(m => m.IdiomID == id);
            _context.Idiom.Remove(idiom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdiomExists(int id)
        {
            return _context.Idiom.Any(e => e.IdiomID == id);
        }
    }
}
