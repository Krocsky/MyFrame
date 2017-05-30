using Common;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 系统设置逻辑
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly EFDbContext _context;
        private readonly IRepository<Setting> _settingRepository;

        public SettingService(EFDbContext context, IRepository<Setting> settingRepository)
        {
            this._context = context;
            this._settingRepository = settingRepository;
        }

        #region 系统设置

        /// <summary>
        /// 根据Id获取设置
        /// </summary>
        /// <param name="SettingId">设置Id</param>
        /// <returns></returns>
        public Setting FindSettingById(int SettingId)
        {
            return _settingRepository.GetById(SettingId);
        }

        /// <summary>
        /// 获取有效的设置
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Setting> Settings
        {
            get
            {
                return _settingRepository.Table.Where(n => !n.IsDelete);
            }
        }

        /// <summary>
        /// 导入设置
        /// </summary>
        /// <param name="Setting">设置实体</param>
        /// <returns></returns>
        public void InsertSetting(Setting Setting)
        {
            Setting.CreatedTime = DateTime.Now;
            Setting.IsDelete = false;
            _settingRepository.Insert(Setting);
        }

        /// <summary>
        /// 异步批量导入设置
        /// </summary>
        /// <param name="Settings">设置集合</param>
        /// <returns></returns>
        public void InsertSetting(IEnumerable<Setting> Settings)
        {
            _settingRepository.Insert(Settings);
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="Setting">设置实体</param>
        /// <returns></returns>
        public void UpdateSetting(Setting Setting)
        {
            _settingRepository.UpdateAsync(Setting);
        }

        /// <summary>
        /// 异步批量更新设置
        /// </summary>
        /// <param name="Settings">设置集合</param>
        /// <returns></returns>
        public void UpdateSetting(IEnumerable<Setting> Settings)
        {
            _settingRepository.UpdateAsync(Settings);
        }

        /// <summary>
        /// 判断设置名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExists(string name)
        {
            if (_settingRepository.FirstOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;
            return true;
        }

        /// <summary>
        /// 根据名称获取设置
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public Setting FindSettingByName(string name)
        {
            return _settingRepository.FirstOrDefault(n=>n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion
    }
}
