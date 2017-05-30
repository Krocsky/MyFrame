using System;

namespace Common
{
    /// <summary>
    /// 推送实体
    /// </summary>
    public partial class Notification : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 推送状态
        /// </summary>
        public NoticeStatus PushStatus { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushTime { get; set; }

        /// <summary>
        /// 外键表
        /// </summary>
        public virtual User UserTable { get; set; }
    }
}
