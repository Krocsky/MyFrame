using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class
    {

    }

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 根据主键id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(TPrimaryKey id);

        /// <summary>
        /// 根据主键id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entities"></param>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entities"></param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entities"></param>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entities"></param>
        Task DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 获取符合条件的第一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取第一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity FirstOrDefault(TPrimaryKey id);

        /// <summary>
        /// 获取符合条件的第一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取第一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        /// 获取数据表
        /// </summary>
        IQueryable<TEntity> Table { get; }

        ///// <summary>
        ///// 获取无跟踪的数据表
        ///// </summary>
        //IQueryable<TEntity> TableNoTracking { get; }
    }
}
