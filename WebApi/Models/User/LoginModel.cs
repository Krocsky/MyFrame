using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 登录model
    /// </summary>
    public partial class LoginModel
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Display(Name = "手机号")]
        [Required(ErrorMessage = "手机号不能为空")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public string EncryptedPassword { get; set; }
    }
}
