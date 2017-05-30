using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    /// <summary>
    /// 用户实名验证
    /// </summary>
    public partial class VerificationModel
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        [Required(ErrorMessage = "真实姓名不能为空")]
        public string TrueName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        [Required(ErrorMessage = "身份证号不能为空")]
        public string IdentityCode { get; set; }
    }
}
