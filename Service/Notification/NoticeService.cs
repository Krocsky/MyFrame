using Common;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 推送接口
    /// </summary>
    public class NoticeService : INoticeService
    {
        #region Ctor

        private readonly EFDbContext _context;
        private readonly IRepository<Notification> _noticeRepository;

        public NoticeService(EFDbContext context, IRepository<Notification> noticeRepository)
        {
            this._context = context;
            this._noticeRepository = noticeRepository;
        }

        #endregion

        #region 推送逻辑

        /// <summary>
        /// 根据Id获取推送
        /// </summary>
        /// <param name="NoticeId">推送Id</param>
        /// <returns></returns>
        public Notification FindNoticeById(int NoticeId)
        {
            return _noticeRepository.GetById(NoticeId);
        }

        /// <summary>
        /// 获取有效的推送
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Notification> Notices
        {
            get
            {
                return _noticeRepository.Table.Where(n => !n.IsDelete);
            }
        }

        /// <summary>
        /// 导入推送
        /// </summary>
        /// <param name="Notice">推送实体</param>
        /// <returns></returns>
        public void InsertNotice(Notification Notice)
        {
            Notice.CreateTime = DateTime.Now;
            Notice.IsDelete = false;
            _noticeRepository.Insert(Notice);
        }

        /// <summary>
        /// 异步批量导入推送
        /// </summary>
        /// <param name="Notices">推送集合</param>
        /// <returns></returns>
        public void InsertNotice(IEnumerable<Notification> Notices)
        {
            _noticeRepository.InsertAsync(Notices);
        }

        /// <summary>
        /// 更新推送
        /// </summary>
        /// <param name="Notice">推送实体</param>
        /// <returns></returns>
        public void UpdateNotice(Notification Notice)
        {
            _noticeRepository.UpdateAsync(Notice);
        }

        /// <summary>
        /// 异步批量更新推送
        /// </summary>
        /// <param name="Notices">推送集合</param>
        /// <returns></returns>
        public void UpdateNotice(IEnumerable<Notification> Notices)
        {
            _noticeRepository.UpdateAsync(Notices);
        }

        #endregion
    }
}
