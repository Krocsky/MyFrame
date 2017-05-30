using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace System.Linq
{
    /// <summary>
    /// Linq扩展方法
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// 第一个参数为True,则执行当前语句。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 第一个参数为True,则执行当前语句。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}