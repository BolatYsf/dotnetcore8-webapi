using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using App.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Products
{
    public class ProductRepository(AppDbContext dbContext) : GenericRepository<Product,int>(dbContext), IProductRepository
    {
        public Task<List<Product>> GetTopPriceProductsAsync(int count)
        {
            return Context.Products.OrderByDescending(p => p.Price).Take(count).ToListAsync();
        }
    }
}
