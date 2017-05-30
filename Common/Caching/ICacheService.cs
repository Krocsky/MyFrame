namespace Common.Caching
{
    /// <summary>
    /// 缓存服务接口
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 根据key获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime">分钟</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 使用正则表达式移除缓存
        /// </summary>
        /// <param name="pattern"></param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 是否设置了某个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(string key);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void Clear();
    }
}
