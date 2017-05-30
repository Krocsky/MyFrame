namespace Common
{
    /// <summary>
    /// 自运行任务控制器
    /// </summary>
    public interface ITaskScheduler
    {
        /// <summary>
        /// 开始任务
        /// </summary>
        void Start();

        /// <summary>
        /// 停止任务
        /// </summary>
        void Stop();

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task"></param>
        void Update(QuartzTask task);

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="task">任务</param>
        void SaveTaskStatus(QuartzTask task);

        /// <summary>
        /// 重启所有任务
        /// </summary>
        void ResumeAll();

        /// <summary>
        /// 获取单个任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QuartzTask GetTask(int id);

        /// <summary>
        /// 运行单个任务
        /// </summary>
        /// <param name="Id">任务Id</param>
        void Run(int Id);
    }
}
