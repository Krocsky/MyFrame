using System;
using System.Linq;
using System.ComponentModel;

namespace Common
{
    public delegate void EventHandler<T>(T sender);

    /// <summary>
    /// 事件接口
    /// </summary>
    public class EventBus<T>
    {
        private static EventBus<T> _eventBus = null;
        private readonly object sync = new object();
        private static EventHandlerList Handlers = new EventHandlerList();

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static EventBus<T> Instance()
        {
            return _eventBus ?? (_eventBus = new EventBus<T>());
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        public void Publish(T entity, string key)
        {
            var handler = Handlers[key];
            if (handler != null)
            {
                Delegate[] delegates = handler.GetInvocationList();
                foreach (EventHandler<T> dele in delegates)
                {
                    try
                    {
                        dele.BeginInvoke(entity, null, null);
                    }
                    catch { }
                }
            }
        }


        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="key"></param>
        public void Subscribe(EventHandler<T> handler, string key)
        {
            Handlers.AddHandler(key, handler);
        }
    }
}
