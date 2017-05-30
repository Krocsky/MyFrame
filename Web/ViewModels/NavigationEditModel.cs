using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class NavigationEditModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 父级导航Id
        /// </summary>
        [Display(Name = "父级导航")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 导航名称
        /// </summary>
        [Display(Name = "导航名称")]
        [Required(ErrorMessage = "请输入导航名称")]
        public string Name { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        [Display(Name = "路由名称")]
        public string RouteName { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [Display(Name = "链接")]
        public string Url { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [Display(Name = "权限名称")]
        [Required(ErrorMessage = "请输入权限名称")]
        public string PermissionName { get; set; }

        /// <summary>
        /// 图标名称
        /// </summary>
        [Display(Name = "图标名称")]
        public string IconName { get; set; }

        /// <summary>
        ///排序序号
        /// </summary>
        [Display(Name = "排序序号")]
        public int DisplayOrder { get; set; }

        /// <summary>
        ///是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [Required(ErrorMessage = "请选择菜单是否启用")]
        public bool IsEnabled { get; set; }
    }
}