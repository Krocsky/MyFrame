using Common;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 推送接口
    /// </summary>
    public interface INoticeService
    {
        #region 推送逻辑

        /// <summary>
        /// 根据Id获取推送
        /// </summary>
        /// <param name="NoticeId">推送Id</param>
        /// <returns></returns>
        Notification FindNoticeById(int NoticeId);

        /// <summary>
        /// 获取有效的推送
        /// </summary>
        /// <returns></returns>
        IQueryable<Notification> Notices
        {
            get;
        }

        /// <summary>
        /// 导入推送
        /// </summary>
        /// <param name="Notice">推送实体</param>
        /// <returns></returns>
        void InsertNotice(Notification Notice);

        /// <summary>
        /// 异步批量导入推送
        /// </summary>
        /// <param name="Notices">推送集合</param>
        /// <returns></returns>
        void InsertNotice(IEnumerable<Notification> Notices);

        /// <summary>
        /// 更新推送
        /// </summary>
        /// <param name="Notice">推送实体</param>
        /// <returns></returns>
        void UpdateNotice(Notification Notice);

        /// <summary>
        /// 异步批量更新推送
        /// </summary>
        /// <param name="Notices">推送集合</param>
        /// <returns></returns>
        void UpdateNotice(IEnumerable<Notification> Notices);

        #endregion
    }
}
