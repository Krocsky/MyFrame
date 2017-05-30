using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 分类
    /// </summary>
    public partial class Category : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 父级分类Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 所属学校Id
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        /// 父级分类
        /// </summary>
        public virtual Category Parent { get; set; }

        /// <summary>
        /// 子级类别
        /// </summary>
        public virtual ICollection<Category> Children { get; set; }
    }
}
