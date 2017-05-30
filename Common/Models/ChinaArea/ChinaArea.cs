using System;

namespace Common
{
    /// <summary>
    /// 中国省市区表
    /// 来源是从github上找的
    /// </summary>
    public partial class ChinaArea : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 地址类型
        /// </summary>
        public AreaType AreaType { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        public string ParentCode { get; set; }
    }
}
