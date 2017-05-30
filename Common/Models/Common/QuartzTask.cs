using System;

namespace Common
{
    /// <summary>
    /// 自运行任务
    /// </summary>
    public partial class QuartzTask : IEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 任务的执行时间规则
        /// </summary>
        public string TaskRule { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        /// <remarks>用于任务实例化</remarks>
        public string ClassType { get; set; }

        /// <summary>
        /// 上次执行开始时间
        /// </summary>
        public DateTime? LastStart { get; set; }

        /// <summary>
        /// 上次执行结束时间
        /// </summary>
        public DateTime? LastEnd { get; set; }

        /// <summary>
        /// 上次任务执行状态
        /// </summary>
        /// <remarks>true-成功/false-失败</remarks>
        public bool? LastIsSuccess { get; set; }

        /// <summary>
        /// 上次执行失败信息
        /// </summary>
        public string LastFailMessage { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextStart { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// 获取规则指定部分
        /// </summary>
        /// <param name="rulePart">规则组成部分</param>
        /// <returns></returns>
        public string GetRulePart(RulePart rulePart)
        {
            if (string.IsNullOrEmpty(TaskRule))
            {
                return RulePart.dayofweek == rulePart ? null : "1";
            }

            string part = TaskRule.Split(' ').GetValue((int)rulePart).ToString();
            if (part == "*" || part == "?")
                return RulePart.dayofweek == rulePart ? null : "1";

            if (part.Contains("/"))
            {
                return part.Substring(part.IndexOf("/") + 1);
            }

            return part;
        }
    }

    /// <summary>
    /// 任务规则组成部分
    /// </summary>
    public enum RulePart
    {
        /// <summary>
        /// 秒域
        /// </summary>
        seconds = 0,
        /// <summary>
        /// 分钟域
        /// </summary>
        minutes = 1,
        /// <summary>
        /// 小时域
        /// </summary>
        hours = 2,
        /// <summary>
        /// 日期域
        /// </summary>
        day = 3,
        /// <summary>
        /// 规则月部分 
        /// </summary>
        mouth = 4,
        /// <summary>
        /// 星期域
        /// </summary>
        dayofweek = 5
    }

    /// <summary>
    /// 任务频率
    /// </summary>
    public enum TaskFrequency
    {
        /// <summary>
        /// 每周
        /// </summary>
        Weekly = 0,

        /// <summary>
        /// 每月
        /// </summary>
        PerMonth = 1,

        /// <summary>
        /// 每天
        /// </summary>
        EveryDay = 2,
    }
}

