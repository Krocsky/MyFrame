using Common;
using Common.Utilities;
using Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 附件业务逻辑
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public AttachmentService(IRepository<Attachment> attachmentRepository)
        {
            this._attachmentRepository = attachmentRepository;
        }

        /// <summary>
        /// 根据Id获取附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Attachment FindById(int id)
        {
            return _attachmentRepository.GetById(id);
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="tenantType"></param>
        /// <param name="itemId"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public List<Attachment> GetAttachments(UserType tenantType, int itemId, MediaType? mediaType = null)
        {
            var attachments = from a in _attachmentRepository.Table
                              where a.TenantType == tenantType && a.ItemId == itemId
                              select a;

            if (mediaType.HasValue)
            {
                switch (mediaType.Value)
                {
                    case MediaType.Image:
                        attachments = attachments.Where(n => n.ContentType.StartsWith("image"));
                        break;
                    case MediaType.Audio:
                        attachments = attachments.Where(n => n.ContentType.StartsWith("audio"));
                        break;
                    case MediaType.Video:
                        attachments = attachments.Where(n => n.ContentType.StartsWith("video"));
                        break;
                    case MediaType.Flash:
                        attachments = attachments.Where(n => n.ContentType.IndexOf("x-shockwave-flash") != -1);
                        break;
                    case MediaType.Document:
                        attachments = attachments.Where(n => n.ContentType.StartsWith("application/vnd.") || n.ContentType.IndexOf("application/msword") != -1);
                        break;
                    case MediaType.Other:
                        attachments = attachments.Where(n => n.ContentType.StartsWith("unknown"));
                        break;
                }
            }

            return attachments.ToList();
        }

        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="fs"></param>
        public void Create(Attachment attachment, Stream stream)
        {
            if (attachment == null || stream == null)
            {
                return;
            }

            string fullPath = GetSavePath(attachment);
            var virtualPath = GetSavePathVirtual(attachment);

            using (FileStream outStream = File.OpenWrite(fullPath))
            {
                byte[] buffer = new byte[stream.Length > 65536 ? 65536 : stream.Length];

                int readedSize;
                while ((readedSize = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, readedSize);
                }

                outStream.Flush();
                outStream.Close();
            }
            attachment.FilePath = virtualPath;
            _attachmentRepository.Insert(attachment);
        }

        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="attachment"></param>
        public void Update(Attachment attachment)
        {
            _attachmentRepository.Update(attachment);
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachment"></param>
        public void Delete(Attachment attachment)
        {
            _attachmentRepository.Delete(attachment);
            string filePath = GetSavePath(attachment);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// 获取附件相对路径
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public string GetRelativePath(Attachment attachment)
        {
            string[] datePaths = attachment.CreatedTime.ToString("yyyy-MM-dd").Split('-').ToArray();
            string datePath = string.Join(new string(Path.DirectorySeparatorChar, 1), datePaths);

            string path = WebHelper.ResolveUrl(string.Format("~/Upload/{0}/{1}/{2}", attachment.TenantType, datePath, attachment.OriginalName));
            return path;
        }

        /// <summary>
        /// 转换临时附件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantType"></param>
        /// <param name="itemId"></param>
        public void ToggleTempAttachment(int userId, UserType tenantType, int itemId)
        {
            var tempAttachment = from a in _attachmentRepository.Table
                                 where a.UserId == userId && a.TenantType == tenantType
                                 select a;

            List<Attachment> list = tempAttachment.ToList();

            foreach(var attachment in list)
            {
                attachment.ItemId = itemId;
            }

            _attachmentRepository.Update(list);
        }

        #region helper

        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        private string GetSavePath(Attachment attachment)
        {
            string[] datePaths = attachment.CreatedTime.ToString("yyyy-MM-dd").Split('-').ToArray();
            string datePath = string.Join(new string(Path.DirectorySeparatorChar, 1), datePaths);

            string path = WebHelper.GetPhysicalFilePath(string.Format("~/Upload/{0}/{1}/", attachment.TenantType, datePath));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullPath = string.Format("{0}{1}", path, attachment.OriginalName);
            return fullPath;
        }

        /// <summary>
        /// 获取文件服务器存储地址
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        private string GetSavePathVirtual(Attachment attachment)
        {
            string[] datePaths = attachment.CreatedTime.ToString("yyyy-MM-dd").Split('-').ToArray();
            string datePath = string.Join(new string(Path.DirectorySeparatorChar, 1), datePaths).Replace('\\', '/');

            string path = string.Format("/Upload/{0}/{1}/", attachment.TenantType, datePath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullPath = string.Format("{0}{1}", path, attachment.OriginalName);
            return fullPath;
        }

        #endregion
    }
}
