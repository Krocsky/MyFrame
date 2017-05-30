using System;
using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    /// <summary>
    /// 权限
    /// </summary>
    public partial class PermissionEditModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入权限名称")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
    }
}
