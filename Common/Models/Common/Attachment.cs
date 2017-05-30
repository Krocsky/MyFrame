using System;
using System.IO;
using System.Web;

namespace Common
{
    /// <summary>
    /// 附件
    /// </summary>
    public partial class Attachment : IEntity
    {
        public Attachment() { }

        public Attachment(HttpPostedFileBase hpf)
        {
            this.FileName = Path.GetFileName(hpf.FileName);
            this.FileLength = hpf.ContentLength;
            this.CreatedTime = DateTime.Now;

            if (!string.IsNullOrEmpty(hpf.ContentType))
                this.ContentType = hpf.ContentType;
            else
                this.ContentType = "unknown/unknown";

            this.OriginalName = this.CreatedTime.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 10000).ToString() + Path.GetExtension(this.FileName);
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 实际存储文件名
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }

        /// <summary>
        /// 附件所属(根据用户类型进行区分)
        /// </summary>
        public UserType TenantType { get; set; }

        /// <summary>
        /// 关联项Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 文件服务器地址
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 附件上传用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 附件上传用户
        /// </summary>
        public virtual User User { get; set; }
    }
}
