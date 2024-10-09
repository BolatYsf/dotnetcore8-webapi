using AutoMapper;
using System.Net;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using App.Application.Contracts.Caching;
using App.Application.Contracts.ServiceBus;
using App.Domain.Events;

namespace App.Application.Features.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork ,IMapper mapper ,ICacheService cacheService,IServiceBus serviceBus) : IProductService
    {

        private const string ProductListCacheKey = "ProductListCacheKey";
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            //var productAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            var productAsDto = mapper.Map<List<ProductDto>>(products);

            return (new ServiceResult<List<ProductDto>>()
            {
                Data = productAsDto
            });
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllAsync()
        {
            // cache aside design pattern
            //1-cache
            //2-from db
            //3.caching data

            var productListAsCached = await cacheService.GetAsync<List<ProductDto>>(ProductListCacheKey);

            if(productListAsCached is not null) return ServiceResult<List<ProductDto>>.Success(productListAsCached);

            var products = await productRepository.GetAllAsync();



            // manuel mapping
            //var productAsResponse = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            // auto mapper

            var productAsResponse = mapper.Map<List<ProductDto>>(products);

            await cacheService.AddAsync(ProductListCacheKey, productAsResponse,TimeSpan.FromMinutes(2));

            return ServiceResult<List<ProductDto>>.Success(productAsResponse);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber , int pageSize)
        {

            //int skip = (pageNumber - 1)* pageSize;

            var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);

            // manuel mapping

            //var productAsDto = products.Select(p=> new ProductDto
            //(
            //    p.Id,
            //    p.Name,
            //    p.Price,
            //    p.Stock
            //)).ToList();

            // automapper

            var productAsDto = mapper.Map<List<ProductDto>>(products);



            return ServiceResult<List<ProductDto>>.Success(productAsDto);
        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
            }

            //var productsAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);

            var productsAsDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productsAsDto)!;
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {

            //throw new CriticalException("a critical error occurred");

            // second method manuel async validation .. best practice
            var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Product name already exists", HttpStatusCode.BadRequest);
            }

            // manuel mapping
            //var product = new Product
            //{
            //    Name = request.Name,
            //    Price = request.Price,
            //    Stock = request.Stock,
            //};

            // mapper

            var product = mapper.Map<Product>(request);



            await productRepository.AddAsync(product);

            await unitOfWork.SavechangesAsync();

            // send event to rabbitmq

            await serviceBus.PublishAsync(new ProductAddedEvent(product.Id, product.Name ,product.Price));


            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            // Fast Fail 
            // Guard Clauses
            // i dont response update and delete requests
            // i didnt write updateproductresponse record

            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != product.Id);

            if (isProductNameExist)
            {
                return ServiceResult.Fail("Product name already exists", HttpStatusCode.BadRequest);
            }

            //product.Price = request.Price;
            //product.Stock = request.Stock;
            //product.Name = request.Name;

            product= mapper.Map(request, product);

            productRepository.Update(product);
            await unitOfWork.SavechangesAsync();

            // response is 204 no content
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }


        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found",HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;

            productRepository.Update(product);
            await unitOfWork.SavechangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found ", HttpStatusCode.NotFound);
            }

            productRepository.Delete(product);

            await unitOfWork.SavechangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);


        }
    }
}
