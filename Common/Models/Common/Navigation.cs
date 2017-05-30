using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 导航菜单
    /// </summary>
    public partial class Navigation : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 父级导航Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 导航名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        public string IconName { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 父级导航
        /// </summary>
        public virtual Navigation Parent { get; set; }

        /// <summary>
        /// 子级导航
        /// </summary>
        public virtual ICollection<Navigation> Children { get; set; }
    }
}
