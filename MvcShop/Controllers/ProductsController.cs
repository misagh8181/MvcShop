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
    public class ProductsController : Controller
    {
        private readonly MvcShopContext _context;
        
        public ProductsController(MvcShopContext context)
        {
            _context = context;
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
        
        public int FindParentCategoryId(int? Id)
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
            return category.ParentId;
        }
        
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var categories=_context.Category.ToList();
            ViewBag.MyList = categories;
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var thisProduct = _context.Product.FirstOrDefault(m => m.Id==id);
            var category = _context.Category.FirstOrDefault(m => m.Id==thisProduct.CategoryId);
            List<string> fieldAndValues = new List<string>();
            var fields = _context.Field.ToList();
            var productFieldValues = _context.ProductFieldValue.ToList();
            foreach (var item in productFieldValues)
            {
                if (item.ProductId==id)
                {
                    foreach (var element in fields)
                    {
                        if (element.Id == item.FieldId)
                        {
                            fieldAndValues.Add(element.Name);
                            fieldAndValues.Add(item.Value);
                        }
                    }
                }
            }

            ViewData["CatName"] = category.Category_Name;
            ViewBag.MyList = fieldAndValues;
            
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(int? Id)
        {
            if (Id == null)
            {
                ViewData["CategoryId"] = 0;
            }
            else
            {
                ViewData["CategoryId"] = Id;
            }

            var myCategories = new List<Category>();
            int? temp = Id;
            while(true)
            {
                if (FindParentCategoryId(temp)==0)
                {
                    myCategories.Add(FindCategory(temp));
                    break;
                }
                myCategories.Add(FindCategory(temp));
                temp = FindParentCategoryId(temp);
            }

            int? variableId;
            var categoryFields = _context.CategoryField.ToList();
            var fields = new List<Field>();
            foreach (var cat in myCategories)
            {
                variableId = cat.Id;
                foreach (var item in categoryFields)
                {
                    if (variableId == item.CategoryId)
                    {
                        var fieldsContext = _context.Field.ToList();
                        foreach (var element in fieldsContext)
                        {
                            if (element.Id==item.FieldId) 
                            {
                                fields.Add(element);
                            } 
                        }
                    }
                }
            }
            ViewBag.MyList = fields;
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Product_Name,Color,Description,Price,Image")] Product product,params String[] fieldValues)
        {
            if (!ModelState.IsValid) return View(product);
            
            _context.Add(product);
            await _context.SaveChangesAsync();
            
            for (int i = 0; i < fieldValues.Length; i+=2)
            {
                ProductFieldValue pfd;
                if (fieldValues[i]==null)
                {
                    pfd = new ProductFieldValue{Value = "None Quantified"};
                }
                else
                {
                    pfd = new ProductFieldValue{Value = fieldValues[i]};
                }
                pfd.FieldId = int.Parse(fieldValues[i+1]);
                pfd.ProductId = product.Id;
                _context.ProductFieldValue.Add(pfd);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var thisProduct = _context.Product.FirstOrDefault(m => m.Id == id);
            List<string> fieldAndValues = new List<string>();
            var fields = _context.Field.ToList();
            var productFieldValues = _context.ProductFieldValue.ToList();
            foreach (var item in productFieldValues)
            {
                if (item.ProductId==id)
                {
                    foreach (var element in fields)
                    {
                        if (element.Id == item.FieldId)
                        {
                            fieldAndValues.Add(element.Name);
                            fieldAndValues.Add(item.Value);
                            fieldAndValues.Add(element.Id.ToString());
                        }
                    }
                }
            }
            
            ViewData["CatId"] = thisProduct.CategoryId;
            ViewBag.MyList = fieldAndValues;
            
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,CategoryId,Product_Name,Color,Description,Price,Image")] Product product,params string[] fieldValues)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    var productFieldValues = _context.ProductFieldValue.ToList();
                    foreach (var item in productFieldValues)
                    {
                        if (item.ProductId==product.Id)
                        {
                            for (int i = 0; i < fieldValues.Length; i+=2)
                            {
                                if (item.FieldId==int.Parse(fieldValues[i+1]))
                                {
                                    item.Value = fieldValues[i];
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var thisProduct = _context.Product.FirstOrDefault(m => m.Id==id);
            var category = _context.Category.FirstOrDefault(m => m.Id==thisProduct.CategoryId);
            List<string> fieldAndValues = new List<string>();
            var fields = _context.Field.ToList();
            var productFieldValues = _context.ProductFieldValue.ToList();
            foreach (var item in productFieldValues)
            {
                if (item.ProductId==id)
                {
                    foreach (var element in fields)
                    {
                        if (element.Id == item.FieldId)
                        {
                            fieldAndValues.Add(element.Name);
                            fieldAndValues.Add(item.Value);
                        }
                    }
                }
            }

            ViewData["CatName"] = category.Category_Name;
            ViewBag.MyList = fieldAndValues;
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var productFieldValue= _context.ProductFieldValue.ToList();
            foreach(var item in productFieldValue)
            {
                if (id==item.ProductId)
                {
                    _context.ProductFieldValue.Remove(item);
                }
            }
            await _context.SaveChangesAsync();
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int? id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
