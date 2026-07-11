using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Repositories
{
    public interface Irepsitory<TEntity> where TEntity : class
    {
        public  Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity?> GetByIdAsync(int id);

        Task<List<TEntity>> GetAllAsync();

        void Delete(TEntity entity);

        public Task UpdateAsync(TEntity entity);

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<List<TResult>> FindAsyncAdvanced<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);

        public Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate);

        public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        public Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        //public  Task<int> SaveChangesAsync();





    }
}
