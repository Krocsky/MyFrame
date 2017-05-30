using Common;
using System;
using System.ComponentModel.DataAnnotations;


namespace Web.ViewModels
{
    /// <summary>
    /// 支付订单修改
    /// </summary>
    public class PayEditModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Display(Name = "支付编号")]
        [Required(ErrorMessage = "请输入支付编号")]
        public int Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Display(Name = "用户编号")]
        [Required(ErrorMessage = "请输入用户编号")]
        public int UserId { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        /// 
        [Display(Name = "支付金额")]
        [Required(ErrorMessage = "请输入支付金额")]
        public double PayNumber { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        /// 
        [Display(Name = "支付类型")]
        public PayType PayType { get; set; }

        /// <summary>
        /// 充值类型
        /// </summary>
        /// 
        [Display(Name = "充值类型")]
        public CostWay CostWay { get; set; }

        /// <summary>
        /// 支付账号
        /// </summary>
        /// 
        [Display(Name = "支付账号")]
        public string PaymentCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// 
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}