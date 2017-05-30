using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.ViewModels
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public partial class PasswordEditModel
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Display(Name = "当前密码")]
        [Required(ErrorMessage = "请输入当前密码")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Display(Name = "新密码")]
        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(50, ErrorMessage = "密码长度不能少于6位", MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        [Display(Name = "确认新密码")]
        [Required(ErrorMessage = "请确认新密码")]
        [Compare("Password", ErrorMessage = "两次输入的新密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}
