using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    /// <summary>
    /// 修改性别
    /// </summary>
    public class ModifyGenderModel
    {
        /// <summary>
        /// 性别类型
        /// </summary>
        [Required(ErrorMessage = "请输入性别类型")]
        public int Gender { get; set; }
    }

    /// <summary>
    /// 修改昵称
    /// </summary>
    public class ModifyNickNameModel
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "请输入昵称")]
        public string NickName { get; set; }
    }
}
