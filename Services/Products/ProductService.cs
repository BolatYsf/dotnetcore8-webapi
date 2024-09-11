using App.Repositories;
using App.Repositories.Products;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository , IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            var productAsDto = products.Select(p=> new ProductDto(p.Id,p.Name,p.Price,p.Stock)).ToList();

            return (new ServiceResult<List<ProductDto>>()
            {
                Data = productAsDto
            });
        }

        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if( product is null)
            {
                return ServiceResult<ProductDto>.Fail("Product not found",HttpStatusCode.NotFound); 
            }

            var productsAsDto = new ProductDto(product!.Id ,product.Name,product.Price,product.Stock);

            return ServiceResult<ProductDto>.Success(productsAsDto!);
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateProductAsync (CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
            };


            await productRepository.AddAsync(product);

            await unitOfWork.SavechangesAsync();

            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));
        }

        public async Task<ServiceResult> UpdateProductAsync (int id,UpdateProductRequest request)
        {
            // Fast Fail 
            // Guard Clauses
            // i dont response update and delete requests

            var product = await productRepository.GetByIdAsync (id);

            if( product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Name = request.Name;
            
            productRepository.Update(product);
            await unitOfWork.SavechangesAsync();

            // response is 204 no content
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
