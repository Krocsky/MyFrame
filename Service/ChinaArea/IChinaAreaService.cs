using Common;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 省市区管理接口
    /// </summary>
    public interface IChinaAreaService
    {
        #region 地区逻辑

        /// <summary>
        /// 根据Id获取地区
        /// </summary>
        /// <param name="ChinaAreaId">地区Id</param>
        /// <returns></returns>
        ChinaArea FindChinaAreaById(int ChinaAreaId);

        /// <summary>
        /// 根据编码获取地区
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        ChinaArea FindChinaAreaByCode(string code);

        /// <summary>
        /// 获取有效的地区
        /// </summary>
        /// <returns></returns>
        IQueryable<ChinaArea> ChinaAreas
        {
            get;
        }

        /// <summary>
        /// 导入地区
        /// </summary>
        /// <param name="ChinaArea">地区实体</param>
        /// <returns></returns>
        void InsertChinaArea(ChinaArea ChinaArea);

        /// <summary>
        /// 异步批量导入地区
        /// </summary>
        /// <param name="ChinaAreas">地区集合</param>
        /// <returns></returns>
        void InsertChinaArea(IEnumerable<ChinaArea> ChinaAreas);

        /// <summary>
        /// 更新地区
        /// </summary>
        /// <param name="ChinaArea">地区实体</param>
        /// <returns></returns>
        void UpdateChinaArea(ChinaArea ChinaArea);

        /// <summary>
        /// 异步批量更新地区
        /// </summary>
        /// <param name="ChinaAreas">地区集合</param>
        /// <returns></returns>
        void UpdateChinaArea(IEnumerable<ChinaArea> ChinaAreas);

        #endregion
    }
}
