using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces.Repositories;
using PMS.Infrastructre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace PMS.Infrastructre.Repository
{
    public class ReposetoryGeneric<TEntity> : Irepsitory<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private DbSet<TEntity> _entity;
        public ReposetoryGeneric(AppDbContext context)
        {

            _context = context;

            _entity = _context.Set<TEntity>();


        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);//return EntityEntry<TEntity>
            return entity;
        }

        public void Delete(TEntity entity)
        {

            _context.Remove(entity);

        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _entity.FindAsync(id);

        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }


        public Task UpdateAsync(TEntity entity)
        {
            _entity.Update(entity);
            return Task.CompletedTask;
        }

        //public async Task UpdateWhereAsync<TProperty>(
        //    Expression<Func<TEntity, bool>> predicate,
        //    Expression<Func<TEntity, TProperty>> property, TProperty value)
        //{
        //    await _entity
        //        .Where(predicate)
        //        .ExecuteUpdateAsync(s =>
        //            s.SetProperty(property,value));
        //}

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.AnyAsync(predicate);
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity
                .Where(predicate)
                .AsTracking()
                .ToListAsync();
        }

        public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity
                .AsTracking()
                .SingleOrDefaultAsync(predicate);
        }

        //Expression convert to sql
        public async Task<List<TResult>> FindAsyncAdvanced<TResult>( Expression<Func<TEntity, bool>> predicate,Expression<Func<TEntity, TResult>> selector)
        {
            return await _entity
                .Where(predicate)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<int> DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entity.Where(predicate).ExecuteDeleteAsync() ;
                
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
            await Task.CompletedTask;
        }

        //public async Task<int> SaveChangesAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}
        //    public async Task UpdateWhereAsync<TProperty>(
        //Expression<Func<TEntity, bool>> predicate,
        //Expression<Func<TEntity, TProperty>> property,
        //TProperty value)
        //    {
        //        await _entity
        //            .Where(predicate)
        //            .ExecuteUpdateAsync(s => s.SetProperty(property, value));
        //    }
    }
}
