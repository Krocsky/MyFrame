using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 消息模板
    /// </summary>
    public class Template : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板类型
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
    }

    public enum MessageType
    {
        /// <summary>
        /// 邮件模板
        /// </summary>
        Email,

        /// <summary>
        /// 短信模板
        /// </summary>
        SMS
    }
}
