using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.ViewModels
{
    /// <summary>
    /// 修改密码【忘记密码情况下】
    /// </summary>
    public partial class ForgetPwdModel
    {

        /// <summary>
        /// 需要更改密码的用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Required(ErrorMessage = "用户Id不能为空")]
        public int ModifyUserId { get; set; }

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
