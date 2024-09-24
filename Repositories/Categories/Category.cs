﻿using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
{
    public sealed class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<Product>? Products { get; set; } 

    }
}
