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
        private readonly ApplicationDbContext applicationDbContext;

        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        //GET INDEX
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await applicationDbContext.Categories.ToListAsync();
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
                applicationDbContext.Add(category);
                await applicationDbContext.SaveChangesAsync();
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
            var category = await applicationDbContext.Categories.FindAsync(id);
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
            var existingCategory = await applicationDbContext.Categories.FindAsync(category.Id);
            if (category is null)
            {
                return NotFound();
            }
            if (existingCategory.Name != category.Name)
            {
                applicationDbContext.Update(category);
                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Please update the category");
            return View(category);
        }

        //POST DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var category = await applicationDbContext.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            applicationDbContext.Remove(category);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET DETAILS
        public IActionResult Details(int? id)
        {
            return RedirectToAction(nameof(Index));
        }

    }
}
