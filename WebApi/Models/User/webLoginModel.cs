using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// web api 网站登录model
    /// </summary>
    public partial class webLoginModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "手机号")]
        [Required(ErrorMessage = "手机号不能为空")]
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public string password { get; set; }

        /// <summary>
        /// 验证信息类型 默认password
        /// </summary>
        [Display(Name = "grant_type")]
        [Required(ErrorMessage = "验证信息类型不能为空")]
        public string grant_type { get; set; }
    }
}
