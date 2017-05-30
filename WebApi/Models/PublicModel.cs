using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi
{
    public class PublicModel
    {
    }

    public class OverOrder
    {
        /// <summary>
        ///  订单编号
        /// </summary>
        [Display(Name = "订单编号")]
        [Required(ErrorMessage = "请填写订单编号")]
        public string OrderId { get; set; }

    }

    /// <summary>
    /// 获取设备秘钥
    /// </summary>
    public class GetDeviceCode
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [Display(Name = "设备编号")]
        [Required(ErrorMessage = "请填写设备编号")]
        public string DeviceName { get; set; }
    }
}