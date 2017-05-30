using System;
using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    /// <summary>
    /// 角色
    /// </summary>
    public partial class RoleEditModel
    {
        public int Id { get; set; }

        public int SchoolId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入角色名称")]
        public string Name { get; set; }

        /// <summary>
        /// 角色职能
        /// </summary>
        [Display(Name = "角色职能")]
        public RoleType Type { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Display(Name = "显示顺序")]
        [Required(ErrorMessage = "请输入显示顺序")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
    }
}
