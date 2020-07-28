using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpiceFactory.Data;
using SpiceFactory.Models;

namespace SpiceFactory.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //GET INDEX
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _dbContext.Categories.ToListAsync();
            return View(categories);
        }

        //GET CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (CategoryExists(category.Name))
                {
                    ViewData["StatusMessage"] = "Error: This category already exists. Please enter another name.";
                    return View(category);
                }
                _dbContext.Add(category);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //GET EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var category = await _dbContext.Categories.FindAsync(id);
            if(category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        //POST EDIT
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (category is null)
            {
                return NotFound();
            }
            if (existingCategory.Name != category.Name)
            {
                _dbContext.Update(category);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Please update the category");
            return View(category);
        }

        // GET: Admin/SubCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET DETAILS
        public IActionResult Details(int? id)
        {
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string name)
        {
            return _dbContext.Categories.Any(c => c.Name == name);

        }
    }
}
