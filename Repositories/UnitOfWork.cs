namespace App.Repositories
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        public Task<int> SavechangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
