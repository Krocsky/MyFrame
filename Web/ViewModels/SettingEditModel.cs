using System;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class SettingEditModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "字段名")]
        public string Name { get; set; }

        /// <summary>
        /// 数值
        /// </summary>
        [Display(Name = "数值")]
        public double Number { get; set; }

        /// <summary>
        /// 字符
        /// </summary>
        [Display(Name = "字符")]
        public string Code { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}