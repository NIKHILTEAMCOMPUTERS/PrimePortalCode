using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Service.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace RMS.Service
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
where TEntity : class, new()
    {
        private readonly RmsDevContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(RmsDevContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        private Expression<Func<TEntity, bool>> PropertyCheckExpression(string propertyName, object propertyValue)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var property = Expression.Property(parameter, propertyName);

            // Check if the property is nullable and if the provided value is also nullable
            if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(property.Type);
                var constant = Expression.Constant(propertyValue, underlyingType);
                var hasValue = Expression.Property(property, "HasValue");
                var equality = Expression.Equal(Expression.Property(property, "Value"), constant);
                var condition = Expression.Condition(hasValue, equality, Expression.Constant(false));
                return Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
            }
            else
            {
                var constant = Expression.Constant(propertyValue, property.Type);
                var equality = Expression.Equal(property, constant);
                return Expression.Lambda<Func<TEntity, bool>>(equality, parameter);
            }
        }

        public virtual Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", CancellationToken cancellationToken = default, bool trackable = true)
        {
            IQueryable<TEntity> query = dbSet;

            Expression<Func<TEntity, bool>> activeFilters = PropertyCheckExpression("Isactive", true);
            if (activeFilters != null)
            {
                query = trackable ? query.Where(activeFilters).AsNoTracking() : query.Where(activeFilters).AsNoTracking();
            }

            if (filter != null)
            {
                query = trackable ? query.Where(filter).AsNoTracking() : query.Where(filter).AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty).AsNoTracking();
            }

            if (orderBy != null)
            {
                return orderBy(query).AsNoTracking().ToListAsync();
            }
            else
            {
                return query.AsNoTracking().ToListAsync();
            }
        }

        public virtual ValueTask<TEntity> GetByIdAsync(object id)
        {
            return dbSet.FindAsync(id);
        }

        public TEntity Add([NotNull] TEntity entity)
        {
            var entityEntryOfTEntity = dbSet.Add(entity);
            return entityEntryOfTEntity.Entity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateStateAlone(TEntity entityToUpdate)
        {
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void DetachEntities(TEntity entityToDetach)
        {
            context.Entry(entityToDetach).State = EntityState.Detached;
        }

        public void DetachEntities(List<TEntity> entitiesToDetach)
        {
            foreach (var entity in entitiesToDetach)
            {
                context.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
