using Microsoft.AspNetCore.Mvc;
using MvcShop.Models;
using System.Diagnostics;
using MvcShop.Data;
using String = System.String;
using Newtonsoft.Json;

namespace MvcShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly MvcShopContext _context;

        private List<Category> IntendedCategories = new List<Category>();
        
        private List<Category> parentCategories = new List<Category>();
        
        public HomeController(MvcShopContext context)
        {
            _context = context;
        }
        
        public void FindParentCategories(int? Id)
        {
            var categoriesList = _context.Category.ToList();
            foreach (var item in categoriesList)
            {
                if (FindCategory(Id).ParentId == item.Id)
                {
                    parentCategories.Add(item);
                    FindParentCategories(item.Id);
                }
            }
        }
        
        public IActionResult Index(int Price = -1, String Name = "", int CategoryChoosed = 0,
            params string[] fieldsChoosed)
        {
            ViewData["Name"] = Name;
            ViewData["Price"] = Price;
            var allCategories = _context.Category.ToList();
            var headCategories = new List<Category>();
            var allProducts = _context.Product.ToList();
            var productsToShow = new List<Product>();
            var categoryFields = _context.CategoryField.ToList();
            var fields = _context.Field.ToList();
            var fieldsToShow = new List<Field>();
            var productFieldValues = _context.ProductFieldValue.ToList();
            
            if (CategoryChoosed != 0)
            {
                parentCategories.Add(FindCategory(CategoryChoosed));
                FindParentCategories(CategoryChoosed);

                foreach (var cat in parentCategories)
                {
                    foreach (var item in categoryFields)
                    {
                        if (item.CategoryId == cat.Id)
                        {
                            foreach (var element in fields)
                            {
                                if (element.Id == item.FieldId)
                                {
                                    fieldsToShow.Add(element);
                                }
                            }
                        }
                    }
                }
            }
            
            if (CategoryChoosed != 0)
            {
                IntendedCategories.Add(FindCategory(CategoryChoosed));
                FindIntendedCategories(CategoryChoosed);
                foreach (var element in IntendedCategories)
                {
                    foreach (var item in allProducts)
                    {
                        if (item.CategoryId == element.Id)
                        {
                            productsToShow.Add(item);
                        }
                    }
                }
            }

            foreach (var item in allCategories)
            {
                if (item.ParentId == 0)
                {
                    headCategories.Add(item);
                }
            }

            List<String> filterForFields=new List<String>();
            
            for (int i = 0; i < fieldsChoosed.Length; i+=2)
            {
                if (fieldsChoosed[i]!="0")
                {
                    filterForFields.Add(fieldsChoosed[i]);
                    filterForFields.Add(fieldsChoosed[i+1]);
                }
            }

            
            ViewData["CurrentCategory"] = CategoryChoosed;
            ViewBag.currentFields = fieldsChoosed;
            ViewBag.filterForFields = filterForFields;
            ViewBag.ProductFieldValues = productFieldValues;
            ViewBag.FieldsToShow = fieldsToShow;
            ViewBag.ProductsToShow = productsToShow;
            ViewBag.HeadCategories = headCategories;
            ViewBag.Categories = allCategories;
            var products = _context.Product.ToList(); // Retrieve all products
            return View(products);
        }
        
        
        public IActionResult ajaxMethod(int categoryId)
        {
            
            var categoryFields = _context.CategoryField.ToList();
            var fields = _context.Field.ToList();
            var fieldsToShow = new List<Field>();
            List<object> fieldsWithSubs = new List<object>();
            
            if (categoryId != 0)
            {
                parentCategories.Add(FindCategory(categoryId));
                FindParentCategories(categoryId);

                foreach (var cat in parentCategories)
                {
                    foreach (var item in categoryFields)
                    {
                        if (item.CategoryId == cat.Id)
                        {
                            foreach (var element in fields)
                            {
                                if (element.Id == item.FieldId)
                                {
                                    fieldsToShow.Add(element);
                                }
                            }
                        }
                    }
                }
            }
            
        
            List<ProductFieldValue> pfv = _context.ProductFieldValue.ToList();
        
            foreach (var item in fieldsToShow)
            {
                List<ProductFieldValue> connector = new List<ProductFieldValue>();
                foreach (var element in pfv)
                {
                    if (element.FieldId == item.Id)
                    {
                        connector.Add(element);
                    }
                }
                connector.RemoveAll(m => m.Value == "None Quantified");
                var pfvValuesWithoutDuplicate = connector.Select(c => c.Value).Distinct().ToList();
                
                fieldsWithSubs.Add(item);
                fieldsWithSubs.Add(pfvValuesWithoutDuplicate);
            }
        
            
            var result = new
            {
                Fields = fieldsWithSubs
            };
        
            
            return Json(result);
        }
        
        public void FindIntendedCategories(int? Id)
        {
        
            if (FindSubCategories(Id).Count != 0)
            {
                foreach (var item in FindSubCategories(Id))
                {
                    IntendedCategories.Add(item);
                    FindIntendedCategories(item.Id);
                }
            }
        }
        
        public Category FindCategory(int? Id)
        {
            Category category = new Category { Category_Name = "" };
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
            List<Category> categories = new List<Category>();
            var categoriesList = _context.Category.ToList();
            foreach (var item in categoriesList)
            {
                if (Id == item.ParentId)
                {
                    categories.Add(item);
                }
            }

            return categories;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}