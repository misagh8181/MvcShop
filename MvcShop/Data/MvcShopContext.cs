using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcShop.Models;

namespace MvcShop.Data
{
    public class MvcShopContext : DbContext
    {
        public MvcShopContext (DbContextOptions<MvcShopContext> options)
            : base(options)
        {
        }

        public DbSet<MvcShop.Models.Product> Product { get; set; } = default!;
        public DbSet<MvcShop.Models.Category> Category { get; set; } = default!;
        public DbSet<MvcShop.Models.Field> Field { get; set; } = default!;
        public DbSet<MvcShop.Models.CategoryField> CategoryField { get; set; } = default!;
        public DbSet<MvcShop.Models.ProductFieldValue> ProductFieldValue { get; set; } = default!;
    }
}
