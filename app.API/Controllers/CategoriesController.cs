﻿using App.Repositories.Categories;
using App.Repositories.Products;
using App.Services.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using App.Services.Filter;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class CategoriesController(ICategoryService categoryService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories() => CreateActionResult(await categoryService.GetAllListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id) => CreateActionResult(await categoryService.GetByIdAsync(id));

        [HttpGet("products")]
        public async Task<IActionResult> GetCategoryWithProducts() => CreateActionResult(await categoryService.GetCategoryWithProducts());

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts(int id) => CreateActionResult(await categoryService.GetCategoryWithProducts(id));

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request) => CreateActionResult(await categoryService.CreateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Category, int>))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateCategoryRequest request,int id) => CreateActionResult(await categoryService.UpdateAsync(id, request));

        [ServiceFilter(typeof(NotFoundFilter<Category, int>))]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id) => CreateActionResult(await categoryService.DeleteAsync(id));




    }
}
