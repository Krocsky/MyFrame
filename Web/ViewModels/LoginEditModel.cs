namespace Web.ViewModels
{
    /// <summary>
    /// 登陆
    /// </summary>
    public partial class LoginEditModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 自动登录
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 登录成功跳转地址
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerCode { get; set; }
    }
}
