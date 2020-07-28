using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpiceFactory.Data;
using Microsoft.EntityFrameworkCore;

namespace SpiceFactory.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim is { })
            {
                var users = await _dbContext.ApplicationUsers.Where(u => u.Id != claim.Value).ToListAsync();
                return View(users);
            }
            return View();
        }
        public async Task<IActionResult> Lock(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var user = await _dbContext.ApplicationUsers.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTime.Now.AddYears(999);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnLock(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var user = await _dbContext.ApplicationUsers.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            user.LockoutEnd = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
