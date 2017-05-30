using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 分页集合接口
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 每页大小
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 数据总数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
