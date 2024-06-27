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
    public class FieldsController : Controller
    {
        private readonly MvcShopContext _context;

        public FieldsController(MvcShopContext context)
        {
            _context = context;
        }

        // GET: Fields
        public async Task<IActionResult> Index()
        {
            var categoryFields=_context.CategoryField.ToList();
            ViewBag.MyList = categoryFields;
            var categories=_context.Category.ToList();
            ViewBag.Categories = categories;
            return View(await _context.Field.ToListAsync());
        }

        // GET: Fields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var categoryField = _context.CategoryField.FirstOrDefault(m => m.FieldId==id);
            var category = _context.Category.FirstOrDefault(m => m.Id == categoryField.CategoryId);
            ViewData["CatName"] = category.Category_Name;
            if (id == null)
            {
                return NotFound();
            }

            var @field = await _context.Field
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // GET: Fields/Create
        public IActionResult Create(int? id)
        {
            if(id == null)
            {
                ViewData["CategoryId"] = 0;
            }
            else
            {
                ViewData["CategoryId"] = id;
            }
            
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Field @field,int? categoryId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@field);
                await _context.SaveChangesAsync();
                var categoryField = new CategoryField();
                categoryField.CategoryId = categoryId;
                categoryField.FieldId = @field.Id;
                _context.CategoryField.Add(categoryField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@field);
        }

        // GET: Fields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @field = await _context.Field.FindAsync(id);
            if (@field == null)
            {
                return NotFound();
            }
            return View(@field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Field @field)
        {
            if (id != @field.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@field);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldExists(@field.Id))
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
            return View(@field);
        }

        // GET: Fields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var categoryField = _context.CategoryField.FirstOrDefault(m => m.FieldId==id);
            var category = _context.Category.FirstOrDefault(m => m.Id == categoryField.CategoryId);
            ViewData["CategoryName"] = category.Category_Name;
            if (id == null)
            {
                return NotFound();
            }

            var @field = await _context.Field
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {   
            var categoryFields = _context.CategoryField.ToList();
            foreach(var item in categoryFields)
            {
                if (id==item.FieldId)
                {
                    _context.CategoryField.Remove(item);
                }
            }
            await _context.SaveChangesAsync();
            
            var @field = await _context.Field.FindAsync(id);
            if (@field != null)
            {
                _context.Field.Remove(@field);
            }
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool FieldExists(int? id)
        {
            return _context.Field.Any(e => e.Id == id);
        }
    }
}
