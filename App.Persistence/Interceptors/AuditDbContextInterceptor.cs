using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Persistence.Interceptors
{
    public class AuditDbContextInterceptor : SaveChangesInterceptor
    {
        // action delegate
        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaivors = new()
        {
            {EntityState.Added, AddBehavior },
            {EntityState.Modified, ModifiedBehavior },
        };
        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.Created = DateTime.Now;
            context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
        }

        private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.Updated = DateTime.Now;
            context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {

            foreach (var entityEntry in eventData.Context.ChangeTracker.Entries().ToList())
            {

                if (entityEntry.Entity is not IAuditEntity auditEntity) continue;


                Behaivors[entityEntry.State](eventData.Context,auditEntity);

                //switch (entityEntry.State)
                //{
                //    case EntityState.Added:

                //        AddBehavior(eventData.Context, auditEntity);

                //        break;

                //    case EntityState.Modified:

                //        ModifiedBehavior(eventData.Context, auditEntity);

                //        break;

                //}


            }


            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
