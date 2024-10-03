using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Categories
{
    public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category,int>(dbContext), ICategoryRepository
    {
        public IQueryable<Category?> GetCategoryWithProductsAsync()
        {
            return  dbContext.Categories.Include(c => c.Products).AsQueryable();
        }

        public Task<Category?> GetCategoryWithProductsAsync(int id)
        {
           return  dbContext.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

        }

        Task<List<Category?>> ICategoryRepository.GetCategoryWithProductsAsync()
        {
            return dbContext.Categories.Include(c=>c.Products).ToListAsync();
        }
    }
}
