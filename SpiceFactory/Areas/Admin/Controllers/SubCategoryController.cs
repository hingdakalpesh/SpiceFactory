using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpiceFactory.Data;
using SpiceFactory.Models;
using SpiceFactory.Areas.Admin.ViewModels;

namespace SpiceFactory.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        //[TempData]
        private string StatusMessage { get; set; }

        public SubCategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Admin/SubCategory
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _dbContext.SubCategories.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/SubCategory/Details/5
        public IActionResult Details(int? id)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/SubCategory/Create
        public IActionResult Create()
        {
            var model = new SubCategoryCreateViewModel()
            {
                Categories = _dbContext.Categories.OrderBy(c => c.Name).ToList(),
                SubCategory = new Models.SubCategory(),
                ExistingSubCategories = _dbContext.SubCategories.OrderBy(s => s.Name).Select(s => s.Name).Distinct().ToList()
            };
            return View(model);
        }

        // POST: Admin/SubCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (SubCategoryExists(model.SubCategory.Name, model.SubCategory.CategoryId))
                {
                    StatusMessage = $"Error: {model.SubCategory.Name} already exists under this category. Please enter another Sub-Category.";
                }
                else
                {
                    _dbContext.Add(model.SubCategory);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            model.Categories = _dbContext.Categories.OrderBy(c => c.Name).ToList();
            model.ExistingSubCategories = _dbContext.SubCategories.OrderBy(s => s.Name).Select(s => s.Name).Distinct().ToList();
            model.StatusMessage = StatusMessage;
            return View(model);
        }

        // GET: Admin/SubCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _dbContext.SubCategories.FindAsync(id);
            var model = new SubCategoryCreateViewModel()
            {
                Categories = _dbContext.Categories.OrderBy(c => c.Name).ToList(),
                SubCategory = subCategory,
                ExistingSubCategories = _dbContext.SubCategories.OrderBy(s => s.Name).Select(s => s.Name).Distinct().ToList()
            };
            return View(model);
        }

        // POST: Admin/SubCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryCreateViewModel model)
        {
           if (ModelState.IsValid)
            {
                if (SubCategoryExists(model.SubCategory.Name, model.SubCategory.CategoryId))
                {
                    StatusMessage = $"Error: {model.SubCategory.Name} already exists under this category. Please enter another Sub-Category.";
                }
                else
                {
                    try
                    {
                        _dbContext.Update(model.SubCategory);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
            }

            model.Categories = _dbContext.Categories.OrderBy(c => c.Name).ToList();
            model.ExistingSubCategories = _dbContext.SubCategories.OrderBy(s => s.Name).Select(s => s.Name).Distinct().ToList();
            model.StatusMessage = StatusMessage;
            return View(model);
        }

        // GET: Admin/SubCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _dbContext.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: Admin/SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _dbContext.SubCategories.FindAsync(id);
            _dbContext.SubCategories.Remove(subCategory);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(string subCategoryName, int categoryId)
        {
            return _dbContext.SubCategories.Include(s => s.Category).Any(s => s.Name == subCategoryName && s.Category.Id == categoryId);
        }

        [ActionName("GetSubCategories")]
        public IActionResult GetSubCategories(int id)
        {
            var subCategoryList = _dbContext.SubCategories.Where(s => s.CategoryId == id).ToList();
            return Json(new SelectList(subCategoryList, "Id", "Name"));
        }
    }
}
