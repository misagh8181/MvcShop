using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcShop.Data;
using MvcShop.Models;
using System;
using System.Linq;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcShopContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcShopContext>>()))
        {
            // Look for any movies.
            if (context.Product.Any())
            {
                return;   // DB has been seeded
            }
            context.Product.AddRange(
                new Product
                {
                    Product_Name = "pajophone",
                    Color = "black",
                    Description = "This is an new product",
                    Price = 230,
                    Image = "https://m.media-amazon.com/images/I/612kg3rGyYL._AC_SX679_.jpg"
                },
                new Product
                {
                    Product_Name = "bag",
                    Color = "brown",
                    Description = "This is an new product",
                    Price = 150,
                    Image = "https://www.crestedandcountry.co.uk/wp-content/uploads/2017/05/BROWN_FRONT_01_v2.jpg"
                },
                new Product
                {
                    Product_Name = "clock",
                    Color = "black",
                    Description = "This is an new product",
                    Price = 90,
                    Image = "https://m.media-amazon.com/images/I/91qKrypuQvL._AC_SL1500_.jpg"
                },
                new Product
                {
                    Product_Name = "pen",
                    Color = "blue",
                    Description = "This is an new product",
                    Price = 12,
                    Image = "https://lamyshop.com.au/cdn/shop/files/LM-314.01_700x700.jpg?v=1693899115"
                }
            ); ;
            context.SaveChanges();
        }
    }
}