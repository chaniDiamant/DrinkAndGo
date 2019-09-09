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
                UserRole = user.Role;
                UserName = user.UserName;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserRole", user.Role);
                return RedirectToAction("ThePreferredDrinks", "Drinks");
            }
            return View(user);
        }
        public IActionResult Login()
        {
            ViewBag.Fail = false;
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("UserName", "");
            HttpContext.Session.SetString("UserRole", "");
            return RedirectToAction("ThePreferredDrinks", "Drinks");
           
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
                HttpContext.Session.SetString("UserName", UserName);
                HttpContext.Session.SetString("UserRole", UserRole);
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

        public async Task<IActionResult> AddToCart(int drinkId)
        {
            if (UserName == null)
                return View("Error", new ErrorViewModel() { Message = "User not connected, cant view cart" });

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == UserName);

            if (user == null)
                return View("Error", new ErrorViewModel() { Message = "User not found, cant view cart" });

            var drink = await _context.Drink.SingleOrDefaultAsync(m => m.DrinkId == drinkId);

            if (drink == null)
                return View("Error", new ErrorViewModel() { Message = "Drink not found" });

            var cart = user.Cart;

            if (_context.Cart_Drinks.Count(c => c.DrinkId == drink.DrinkId && c.UserId == user.Id) == 0)
                cart.Add(new Cart_Drink() { DrinkId = drink.DrinkId, Drink = drink, UserId = user.Id, User = user, DrinkCount = 1 });
            else
            {
                var drinkChosen = _context.Cart_Drinks.FirstOrDefault(c => c.DrinkId == drink.DrinkId && c.UserId == user.Id);
                drinkChosen.DrinkCount++;
                //cart.Add(new Cart_Drink() { DrinkId = drink.DrinkId, Drink = drink, UserId = user.Id, User = user });
            }

            _context.SaveChanges();

            return Redirect(ControllerContext.HttpContext.Request.Headers["Referer"]); // redirect to last page
        }

        public async Task<IActionResult> ViewCart()
        {
            if (UserName == null)
                return View("Error", new ErrorViewModel() { Message = "User not connected, cant view cart" });

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == UserName);

            if (user == null)
                return View("Error", new ErrorViewModel() { Message = "User not found, cant view cart" });


            Dictionary<int, int> drinkToCount = new Dictionary<int, int>();


            var drinks = _context.Cart_Drinks.Where(cd => cd.UserId == user.Id);

            foreach (var cart_Drink in drinks)
                drinkToCount[cart_Drink.DrinkId] = cart_Drink.DrinkCount;
            //if (drinkToCount.ContainsKey(cart_Drink.DrinkId))
            //    drinkToCount[cart_Drink.DrinkId] = drinkToCount[cart_Drink.DrinkId] + 1;
            //else
            //    drinkToCount[cart_Drink.DrinkId] = 1;

            Dictionary<Drink, int> result = new Dictionary<Drink, int>();
            foreach (var itm in drinkToCount)
                result[_context.Drink.FirstOrDefault(m => m.DrinkId == itm.Key)] = itm.Value;


            return View(result);
        }

        public async Task<IActionResult> RemoveFromCart(int drinkId)
        {
            if (UserName == null)
                return View("Error", new ErrorViewModel() { Message = "User not connected, cant view cart" });

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserName == UserName);

            if (user == null)
                return View("Error", new ErrorViewModel() { Message = "User not found, cant view cart" });

            var drink = await _context.Drink.SingleOrDefaultAsync(m => m.DrinkId == drinkId);

            if (drink == null)
                return View("Error", new ErrorViewModel() { Message = "Drink not found" });

            var drinkChosen = _context.Cart_Drinks.FirstOrDefault(c => c.DrinkId == drink.DrinkId && c.UserId == user.Id);
            drinkChosen.DrinkCount--;

            if (drinkChosen.DrinkCount == 0)
                user.Cart.Remove(drinkChosen);

            _context.SaveChanges();

            return RedirectToAction("ViewCart");
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserName == id);
        }
    }
}
