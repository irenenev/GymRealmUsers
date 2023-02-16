using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymRealmUsers.Data;
using Microsoft.Extensions.Logging;
using GymRealmUsers.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GymRealmUsers.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["PhoneSortParm"] = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
            ViewData["CitySortParm"] = sortOrder == "city_asc" ? "city_desc" : "city_asc";
            var users = from u in _context.Users select u;
            switch (sortOrder)
            {
                case "id_desc":
                    users = users.OrderByDescending(s => s.Id);
                    break;
                case "email_asc":
                    users = users.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case "name_asc":
                    users = users.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case "phone_asc":
                    users = users.OrderBy(s => s.Telephone);
                    break;
                case "phone_desc":
                    users = users.OrderByDescending(s => s.Telephone);
                    break;
                case "city_asc":
                    users = users.OrderBy(s => s.City);
                    break;
                case "city_desc":
                    users = users.OrderByDescending(s => s.City);
                    break;
                default:
                    users = users.OrderBy(s => s.Id);
                    break;
            }
            return View(await users.AsNoTracking().ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var user = await _context.Users.FindAsync(id);

            if (user == null) return NotFound();
            
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Name,Telephone,City")] User user)
        {
            if (id != user.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            
            if (user == null) return NotFound();
            
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userByContext = HttpContext.User;
            var userEmail = userByContext.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Name).Value;
            var user = await _context.Users.FindAsync(id);
            if (user.Email != userEmail)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(user => user.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
