using System.Collections.Generic;
using Common;
using System.IO;

namespace Service
{
    /// <summary>
    /// 附件业务逻辑接口
    /// </summary>
    public interface IAttachmentService
    {
        /// <summary>
        /// 根据Id获取附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Attachment FindById(int id);

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="UserType"></param>
        /// <param name="itemId"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        List<Attachment> GetAttachments(UserType tenantType, int itemId, MediaType? mediaType = null);

        /// <summary>
        /// 创建附件
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="fs"></param>
        void Create(Attachment attachment, Stream fs);

        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="attachment"></param>
        void Update(Attachment attachment);

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="attachment"></param>
        void Delete(Attachment attachment);

        /// <summary>
        /// 获取附件相对路径
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        string GetRelativePath(Attachment attachment);

        /// <summary>
        /// 转换临时附件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantType"></param>
        /// <param name="itemId"></param>
        void ToggleTempAttachment(int userId, UserType tenantType, int itemId);
    }
}
