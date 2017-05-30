using System;

namespace Common
{
    /// <summary>
    /// 参数表
    /// </summary>
    public partial class Setting : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        public double Number { get; set; }

        /// <summary>
        /// 字符
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
