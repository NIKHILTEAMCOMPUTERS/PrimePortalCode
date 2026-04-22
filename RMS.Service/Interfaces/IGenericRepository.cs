using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace RMS.Service.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", CancellationToken cancellationToken = default, bool trackable = true);

        ValueTask<TEntity> GetByIdAsync(object id);

        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        void UpdateStateAlone(TEntity entityToUpdate);
        void DetachEntities(TEntity entityToDetach);
        void DetachEntities(List<TEntity> entitiesToDetach);
        TEntity Add([NotNull] TEntity entity);
    }
}
