using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrinkAndGo.Models;

namespace DrinkAndGo.Controllers
{
    public class DrinksController : Controller
    {
        private readonly DrinkAndGoContext _context;

        public DrinksController(DrinkAndGoContext context)
        {
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index()
        {
            var drinkAndGoContext = _context.Drink.Include(d => d.Category);
            return View(await drinkAndGoContext.ToListAsync());
        }
        public async Task<IActionResult> Check()
        {
            var q = from u in _context.Drink
                    group u by u.IsPreferredDrink==true into groups
                    select groups.Count();
            ViewBag.data = "[" + string.Join(",", q.ToList()) + "]";
            return View(await _context.Drink.ToListAsync());
        }
            public async Task<IActionResult> Search(string drinkname, decimal price,bool instok)
        {
            var result = from d in _context.Drink
                         where( d.Name == drinkname && d.Price<price && d.InStock==instok)
                         select d;
            return View(await result.ToListAsync());
        }
        public async Task<IActionResult> ThePreferredDrinks()
        {
            var result = from d in _context.Drink
                         where d.IsPreferredDrink ==true
                         select d;
            return View(await result.ToListAsync());
        }
        public async Task<IActionResult> Alcoholic()
        {
            var result = from d in _context.Drink
                         where d.Category.CategoryId == 1
                         select d;
            return View(await result.ToListAsync());
        }
        public async Task<IActionResult> NonAlcoholic()
        {
            var result = from d in _context.Drink
                         where d.Category.CategoryId == 2
                         select d;
            return View(await result.ToListAsync());
        }

        public async Task<IActionResult> Index1()
        {
            //var q = from d in _context.Drink
            //        select d.Price;

            //ViewBag.data = "[" + string.Join(",", q.ToList()) + "]";

            var result = from d in _context.Drink
                         group d by (d.Price %10== 0) into groups
                         select groups;

            return View(await _context.Drink.ToListAsync());
        }


        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drink
                .Include(d => d.Category)
                .SingleOrDefaultAsync(m => m.DrinkId == id);
            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DrinkId,Name,ShortDescription,LongDescription,Price,ImageUrl,ImageThumbnailUrl,IsPreferredDrink,InStock,CategoryId")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", drink.CategoryId);
            return View(drink);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drink.SingleOrDefaultAsync(m => m.DrinkId == id);
            if (drink == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", drink.CategoryId);
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DrinkId,Name,ShortDescription,LongDescription,Price,ImageUrl,ImageThumbnailUrl,IsPreferredDrink,InStock,CategoryId")] Drink drink)
        {
            if (id != drink.DrinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drink.DrinkId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", drink.CategoryId);
            return View(drink);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drink
                .Include(d => d.Category)
                .SingleOrDefaultAsync(m => m.DrinkId == id);
            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drink = await _context.Drink.SingleOrDefaultAsync(m => m.DrinkId == id);
            _context.Drink.Remove(drink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrinkExists(int id)
        {
            return _context.Drink.Any(e => e.DrinkId == id);
        }
    }
}
