using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
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
    }
}
