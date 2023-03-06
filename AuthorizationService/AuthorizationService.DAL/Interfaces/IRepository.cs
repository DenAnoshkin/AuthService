using AuthorizationService.DAL.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AuthorizationService.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {

        public IEnumerable<TEntity> GetAll();

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        public Task<TEntity?> GetAsync(int id);

        public Task AddAsync(TEntity entity);

        public Task DeleteAsync(int id);

        public Task UpdateAsync(TEntity entity);

    }
}
