using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrinkAndGo.Models;
using Microsoft.AspNetCore.Http;

namespace DrinkAndGo.Controllers
{
    public class UsersController : Controller
    {
        private readonly DrinkAndGoContext _context;
        public static string UserRole = null;
        public static string UserName = null;

        public UsersController(DrinkAndGoContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            //var q = from u in _context.User
            //        select u.Age;

            //ViewBag.data = "[" + string.Join(",", q.ToList()) + "]";

            //var result = from u in _context.User
            //             group u by (u.Age / 10) into groups
            //             select groups;

            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> Check()
        {
            var q = from u in _context.User.Distinct()
                    orderby u.Age
                    select u.Age;
            ViewBag.data = "[" + string.Join(",", q.ToList()) + "]";
            return View(await _context.User.ToListAsync());
        }
        public async Task<IActionResult> Search(string name, decimal age, string type)
        {
            var result = from d in _context.User
                         where (d.UserName == name && d.Age == age && d.Role == type)
                         select d;
            return View(await result.ToListAsync());
        }
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Age,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public IActionResult Login()
        {
            ViewBag.Fail = false;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Password")] User user)
        {
            var result = from u in _context.User
                         where u.UserName == user.UserName && u.Password == user.Password
                         select u;

            if (result.ToList().Count > 0)
            {
                UserRole = result.FirstOrDefault().Role;
                UserName = result.FirstOrDefault().UserName;
                return RedirectToAction("ThePreferredDrinks", "Drinks");
            }

            ViewBag.Fail = true;



            return View(user);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Age,Password")] User user)
        {
            if (id != user.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserName))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddToCart(string userId, int drinkId)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == userId);
            var drink = await _context.Drink.SingleOrDefaultAsync(m => m.DrinkId == drinkId);
            if (user == null || drink == null)
                return NotFound();

            if (user.CartId == null)
            {
                Cart cartNew = new Cart();
                _context.Cart.Add(cartNew);
                user.CartId = cartNew.CartId; // might not work
                _context.SaveChanges();
            }

            var cart = _context.Cart.Find(user.CartId); // returns cart with id 1, with the correct drink
            cart.Drinks.Add(drink);
            _context.SaveChanges();
            return Redirect(ControllerContext.HttpContext.Request.Headers["Referer"]); // redirect to last page
        }

        public async Task<IActionResult> ViewCart(string userId)
        {
            

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == userId);

            if (user.CartId == null)
            {
                Cart cartNew = new Cart();
                _context.Cart.Add(cartNew);
                user.CartId = cartNew.CartId; // might not work
                _context.SaveChanges();
            }
            var cart = await _context.Cart.SingleOrDefaultAsync(m => m.CartId == user.CartId); // return cart with id 1, but drinks are empty

            return View(cart.Drinks);
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserName == id);
        }
    }
}
