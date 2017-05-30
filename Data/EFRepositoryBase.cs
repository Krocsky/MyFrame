using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// EF仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EFRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        public EFRepositoryBase(EFDbContext context)
        {
            this.Context = context;
        }

        public EFDbContext Context { get; set; }
        private DbSet<TEntity> _entities;

        #region Methods

        /// <summary>
        /// 根据主键id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(TPrimaryKey id)
        {
            return this.Entities.Find(id);
        }

        /// <summary>
        /// 根据主键id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await this.Entities.FindAsync(id);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task InsertAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this.Context.BulkInsert(entities);

                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this.Context.BulkInsert(entities);

                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");
                
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entities"></param>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");
                
                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="predicate"></param>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entities = this.Entities.Where(predicate);

                this.Entities.RemoveRange(entities);

                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this.Entities.RemoveRange(entities);

                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        public virtual async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this.Entities.RemoveRange(entities);

                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取符合条件的第一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Entities.FirstOrDefault(predicate);
        }

        /// <summary>
        /// 根据主键获取第一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity FirstOrDefault(TPrimaryKey id)
        {
            return this.Entities.FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// 获取符合条件的第一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        /// <summary>
        /// 根据主键获取第一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取数据表
        /// </summary>
        public virtual IQueryable<TEntity> Table
        {
            get
            {
                return this.Entities;
            }
        }

        ///// <summary>
        ///// 获取无跟踪的数据表
        ///// </summary>
        //public virtual IQueryable<TEntity> TableNoTracking
        //{
        //    get
        //    {
        //        return this.Entities.AsNoTracking();
        //    }
        //}

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = Context.Set<TEntity>();
                return _entities;
            }
        }

        #endregion

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
