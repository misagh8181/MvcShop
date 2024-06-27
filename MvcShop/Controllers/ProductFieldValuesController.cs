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
    public class ProductFieldValuesController : Controller
    {
        private readonly MvcShopContext _context;

        public ProductFieldValuesController(MvcShopContext context)
        {
            _context = context;
        }

        // GET: ProductFieldValues
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductFieldValue.ToListAsync());
        }

        // GET: ProductFieldValues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productFieldValue = await _context.ProductFieldValue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productFieldValue == null)
            {
                return NotFound();
            }

            return View(productFieldValue);
        }

        // GET: ProductFieldValues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductFieldValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,FieldId,Value")] ProductFieldValue productFieldValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productFieldValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productFieldValue);
        }

        // GET: ProductFieldValues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productFieldValue = await _context.ProductFieldValue.FindAsync(id);
            if (productFieldValue == null)
            {
                return NotFound();
            }
            return View(productFieldValue);
        }

        // POST: ProductFieldValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,FieldId,Value")] ProductFieldValue productFieldValue)
        {
            if (id != productFieldValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productFieldValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductFieldValueExists(productFieldValue.Id))
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
            return View(productFieldValue);
        }

        // GET: ProductFieldValues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productFieldValue = await _context.ProductFieldValue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productFieldValue == null)
            {
                return NotFound();
            }

            return View(productFieldValue);
        }

        // POST: ProductFieldValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productFieldValue = await _context.ProductFieldValue.FindAsync(id);
            if (productFieldValue != null)
            {
                _context.ProductFieldValue.Remove(productFieldValue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductFieldValueExists(int? id)
        {
            return _context.ProductFieldValue.Any(e => e.Id == id);
        }
    }
}
