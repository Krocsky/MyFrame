using Common;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 系统设置逻辑
    /// </summary>
    public interface ISettingService
    {
        #region 系统设置

        /// <summary>
        /// 根据Id获取设置
        /// </summary>
        /// <param name="SettingId">设置Id</param>
        /// <returns></returns>
        Setting FindSettingById(int SettingId);

        /// <summary>
        /// 获取有效的设置
        /// </summary>
        /// <returns></returns>
        IQueryable<Setting> Settings
        {
            get;
        }

        /// <summary>
        /// 导入设置
        /// </summary>
        /// <param name="Setting">设置实体</param>
        /// <returns></returns>
        void InsertSetting(Setting Setting);

        /// <summary>
        /// 异步批量导入设置
        /// </summary>
        /// <param name="Settings">设置集合</param>
        /// <returns></returns>
        void InsertSetting(IEnumerable<Setting> Settings);

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="Setting">设置实体</param>
        /// <returns></returns>
        void UpdateSetting(Setting Setting);

        /// <summary>
        /// 异步批量更新设置
        /// </summary>
        /// <param name="Settings">设置集合</param>
        /// <returns></returns>
        void UpdateSetting(IEnumerable<Setting> Settings);

        /// <summary>
        /// 判断设置名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsExists(string name);

        /// <summary>
        /// 根据名称获取设置
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Setting FindSettingByName(string name);

        #endregion
    }
}
