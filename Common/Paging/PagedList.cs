using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common
{
    /// <summary>
    /// 分页集合
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T> where T : class, IEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.OrderByDescending(n => n.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, Expression<Func<T, int>> predicate)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.OrderBy(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, Expression<Func<T, DateTime>> predicate)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.OrderByDescending(predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; private set; }


        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }
    }
}
