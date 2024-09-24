﻿using App.Repositories.Categories;
using System.Security.Principal;

namespace App.Repositories.Products
{
    public sealed class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } =default!;
    }
}
