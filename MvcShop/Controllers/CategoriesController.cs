using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcShop.Data;
using MvcShop.Models;

namespace MvcShop.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MvcShopContext _context;

        private List<Category> CategoriesToDelete = new List<Category>();

        public CategoriesController(MvcShopContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Category cat=_context.Category.FirstOrDefault(m => m.Id==id);
            var categories = _context.Category.ToList();
            if (cat.ParentId == 0)
            {
                ViewData["PId"] = "nothing";
            }
            else
            {
                foreach (var item in categories)
                {
                    if (item.Id==cat.ParentId)
                    {
                        ViewData["PId"] = item.Category_Name;
                    }
                }
            }
            
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create(int? id)
        {
            var categories = _context.Category.ToList();
            int firstParentId=0;
            int secondParentId = 0;
            int thirdParentId = 0;
            foreach (var item in categories)
            {
                if (id==item.Id)
                {
                    firstParentId = item.ParentId;
                }
            }
            
            foreach (var item in categories)
            {
                if (firstParentId==item.Id)
                {
                    secondParentId = item.ParentId;
                }
            }
           
            foreach (var item in categories)
            {
                if (secondParentId==item.Id)
                {
                    thirdParentId = 25;//to make it none null
                }
            }
            
            if (firstParentId != 0 && secondParentId != 0 && thirdParentId != 0)
            {
                TempData["ErrorMessage"] = "You can create a maximum of three sub-categories";
            }
            
            if(id == null)
            {
                ViewData["PId"] = 0;
            }
            else
            {
                ViewData["PId"] = id;
            }
            //var Categories = _context.Category.ToList(); // Retrieve all products*/
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParentId,Category_Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentId,Category_Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Category cat=_context.Category.FirstOrDefault(m => m.Id==id);
            var categories = _context.Category.ToList();
            if (cat.ParentId == 0)
            {
                ViewData["ParentId"] = "nothing";
            }
            else
            {
                foreach (var item in categories)
                {
                    if (item.Id==cat.ParentId)
                    {
                        ViewData["ParentId"] = item.Category_Name;
                    }
                }
            }
            if (id == null)
            {
                return NotFound();
            }
            
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int temp = id;
            CategoriesToDelete.Add(FindCategory(temp));
            FindCategories(temp);
            foreach (var catToDelete in CategoriesToDelete)
            {
                var products = _context.Product.ToList();
                var productsToDelete = new List<Product>();
                foreach(var item in products)
                {
                    if (catToDelete.Id==item.CategoryId)
                    {
                        productsToDelete.Add(item);
                    }
                }   

                DeleteProductFieldValues(productsToDelete);
                await _context.SaveChangesAsync();
            
                _context.Product.RemoveRange(productsToDelete);
                await _context.SaveChangesAsync();
            
                DeleteCategoryFields(catToDelete.Id);
                await _context.SaveChangesAsync();
            
                var category = await _context.Category.FindAsync(catToDelete.Id);
                if (category != null)
                {
                    _context.Category.Remove(category);
                }
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        public void FindCategories(int? temp)
        {
            List<Category> categories = FindSubCategories(temp);
            if (categories.Count==0)
            {
                CategoriesToDelete.Add(FindCategory(temp));
                return;
            }
            foreach (var item in categories)
            {
                CategoriesToDelete.Add(item);
                FindCategories(item.Id);
            }
        }
        
        public Category FindCategory(int? Id)
        {
            Category category=new Category{Category_Name = ""};
            var categoriesList = _context.Category.ToList();
            foreach (var item in categoriesList)
            {
                if (Id == item.Id)
                {
                    category = item;
                }
            }
            return category;
        }
        
        public List<Category> FindSubCategories(int? Id)
        {
            List<Category> subCategories = new List<Category>();
            var categoriesList = _context.Category.ToList();
            foreach (var item in categoriesList)
            {
                if (Id == item.ParentId)
                {
                    subCategories.Add(item);
                }
            }
            return subCategories;
        }
        private void DeleteCategoryFields(int? id)
        {
            var categoryFields = _context.CategoryField.ToList();
            foreach(var item in categoryFields)
            {
                if (id==item.CategoryId)
                {
                    var fields = _context.Field.FirstOrDefault(m => m.Id==item.FieldId);
                    _context.CategoryField.Remove(item);
                    _context.Field.Remove(fields);
                }
            }
        }

        private void DeleteProductFieldValues(List<Product> productsToDelete)
        {
            List<ProductFieldValue> productFieldValues=_context.ProductFieldValue.ToList();
            foreach (var element in productsToDelete)
            {
                foreach (var item in productFieldValues)
                {
                    if (item.ProductId == element.Id)
                    {
                        _context.ProductFieldValue.Remove(item);
                    }
                }
            }
        }


        private bool CategoryExists(int? id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
