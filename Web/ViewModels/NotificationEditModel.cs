using Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class NotificationEditModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }

        /// <summary>
        /// 设备唯一标示
        /// </summary>
        [Display(Name = "推送内容")]
        [Required(ErrorMessage = "请输入推送内容")]
        public string Content { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        [Display(Name = "推送时间")]
        [Required(ErrorMessage = "请选择时间")]
        public DateTime PushTime { get; set; }
    }
}