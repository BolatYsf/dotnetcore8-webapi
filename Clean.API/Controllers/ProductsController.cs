﻿using App.Application.Features.Products;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using Clean.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{

    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult =await productService.GetAllAsync();

            return CreateActionResult(serviceResult);

        }

        // i m using route constraint .. client cant type string or another type 
        [HttpGet("{pageNumber:int}/{pageSize:int}")]

        public async Task<IActionResult> GetPagedAll(int pageNumber , int pageSize) => CreateActionResult(await productService.GetPagedAllListAsync(pageNumber,pageSize));

        // ~/api/products/id
        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));


        [HttpPost]

        public async Task<IActionResult> Create(CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));

        [HttpPatch("stock")]

        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) => CreateActionResult(await productService.UpdateStockAsync(request));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
    }
}
