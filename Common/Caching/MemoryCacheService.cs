using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Common.Caching
{
    /// <summary>
    /// 内存缓存实现
    /// </summary>
    public partial class MemoryCacheService : ICacheService
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// 根据key获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            if (IsSet(key))
                Remove(key);

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 使用正则表达式移除缓存
        /// </summary>
        /// <param name="pattern"></param>
        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in Cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// 是否设置了某个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return Cache.Contains(key);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}