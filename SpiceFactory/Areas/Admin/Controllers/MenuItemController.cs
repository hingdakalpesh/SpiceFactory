using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpiceFactory.Data;
using SpiceFactory.Models;
using SpiceFactory.Areas.Admin.ViewModels;
using System.IO;

namespace SpiceFactory.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnv;

        [BindProperty]
        public MenuItemCreateEditViewModel CreateEditViewModel { get; set; }

        public MenuItemController(ApplicationDbContext dbContext, IWebHostEnvironment hostEnv)
        {
            _dbContext = dbContext;
            _hostEnv = hostEnv;
            CreateEditViewModel = new MenuItemCreateEditViewModel()
            {
                Categories = _dbContext.Categories.ToList(),
                //SubCategories = await _dbContext.SubCategories.ToListAsync(),
                MenuItem = new Models.MenuItem()
            };
        }
        public async Task<IActionResult> Index()
        {
            var menuItems = await _dbContext.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();
            return View(menuItems);
        }

        public IActionResult Create()
        {
            return View(CreateEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(CreateEditViewModel);
            }
            _dbContext.MenuItems.Add(CreateEditViewModel.MenuItem);
            await _dbContext.SaveChangesAsync();

            string rootPath = _hostEnv.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            if (file.Any())
            {
                var extension = Path.GetExtension(file[0].FileName);
                string imgPath = Path.Combine(rootPath, "images", "MenuItems", CreateEditViewModel.MenuItem.Id.ToString() + extension);
                using FileStream fileStream = new FileStream(imgPath, FileMode.Create);
                await file[0].CopyToAsync(fileStream);
                CreateEditViewModel.MenuItem.Image = Path.Combine("images", "MenuItems", CreateEditViewModel.MenuItem.Id.ToString() + extension);
            }
            else
            {
                CreateEditViewModel.MenuItem.Image = Path.Combine("images", "MenuItems", "default.png");
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            CreateEditViewModel.MenuItem = await _dbContext.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).FirstOrDefaultAsync(m => m.Id == id);
            CreateEditViewModel.SubCategories = await _dbContext.SubCategories.Where(m => m.CategoryId == CreateEditViewModel.MenuItem.CategoryId).ToListAsync();
            return View(CreateEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit()
        {
            if (ModelState.IsValid)
            {
                string rootPath = _hostEnv.WebRootPath;
                var file = HttpContext.Request.Form.Files;

                if (file.Any())
                {
                    //delete existing image
                    string existingImgPath = CreateEditViewModel.MenuItem.Image;
                    if (existingImgPath is { })
                    {
                        if (!(existingImgPath.Equals(Path.Combine("images", "MenuItems", "Default.png"))))
                        {
                            if (System.IO.File.Exists(Path.Combine(rootPath, existingImgPath)))
                            {
                                System.IO.File.Delete(Path.Combine(rootPath, existingImgPath));
                            }
                        }
                    }

                    //upload new image
                    var extension = Path.GetExtension(file[0].FileName);
                    string imgPath = Path.Combine(rootPath, "images", "MenuItems", CreateEditViewModel.MenuItem.Id.ToString() + extension);
                    using FileStream fileStream = new FileStream(imgPath, FileMode.Create);
                    await file[0].CopyToAsync(fileStream);
                    CreateEditViewModel.MenuItem.Image = Path.Combine("images", "MenuItems", CreateEditViewModel.MenuItem.Id.ToString() + extension);
                }
                _dbContext.MenuItems.Update(CreateEditViewModel.MenuItem);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(CreateEditViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            MenuItem menuItem = await _dbContext.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).FirstOrDefaultAsync(m => m.Id == id);
            CreateEditViewModel.MenuItem = menuItem;
            return View(CreateEditViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            MenuItem menuItem = await _dbContext.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).FirstOrDefaultAsync(m => m.Id == id);
            CreateEditViewModel.MenuItem = menuItem;
            return View(CreateEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            MenuItem menuItem = await _dbContext.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).FirstOrDefaultAsync(m => m.Id == id);
            string rootPath = _hostEnv.WebRootPath;

            //delete existing image
            string existingImgPath = menuItem.Image;
            if (existingImgPath is { })
            {
                if (!(existingImgPath.Equals(Path.Combine("images", "MenuItems", "Default.png"))))
                {
                    if (System.IO.File.Exists(Path.Combine(rootPath, existingImgPath)))
                    {
                        System.IO.File.Delete(Path.Combine(rootPath, existingImgPath));
                    }
                }
            }
            _dbContext.MenuItems.Remove(menuItem);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
