using Common;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 省市区管理接口
    /// </summary>
    public class ChinaAreaService : IChinaAreaService
    {
        private readonly EFDbContext _context;
        private readonly IRepository<ChinaArea> _chinaAreaRepository;

        public ChinaAreaService(EFDbContext context, IRepository<ChinaArea> chinaAreaRepository)
        {
            this._context = context;
            this._chinaAreaRepository = chinaAreaRepository;
        }

        #region 地区逻辑

        /// <summary>
        /// 根据Id获取地区
        /// </summary>
        /// <param name="ChinaAreaId">地区Id</param>
        /// <returns></returns>
        public ChinaArea FindChinaAreaById(int ChinaAreaId)
        {
            return _chinaAreaRepository.GetById(ChinaAreaId);
        }

        /// <summary>
        /// 根据编码获取地区
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public ChinaArea FindChinaAreaByCode(string code)
        {
            return _chinaAreaRepository.Table.FirstOrDefault(n=>n.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取有效的地区
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<ChinaArea> ChinaAreas
        {
            get
            {
                return _chinaAreaRepository.Table;
            }
        }

        /// <summary>
        /// 导入地区
        /// </summary>
        /// <param name="ChinaArea">地区实体</param>
        /// <returns></returns>
        public void InsertChinaArea(ChinaArea ChinaArea)
        {
            _chinaAreaRepository.Insert(ChinaArea);
        }

        /// <summary>
        /// 异步批量导入地区
        /// </summary>
        /// <param name="ChinaAreas">地区集合</param>
        /// <returns></returns>
        public void InsertChinaArea(IEnumerable<ChinaArea> ChinaAreas)
        {
            _chinaAreaRepository.Insert(ChinaAreas);
        }

        /// <summary>
        /// 更新地区
        /// </summary>
        /// <param name="ChinaArea">地区实体</param>
        /// <returns></returns>
        public void UpdateChinaArea(ChinaArea ChinaArea)
        {
            _chinaAreaRepository.UpdateAsync(ChinaArea);
        }

        /// <summary>
        /// 异步批量更新地区
        /// </summary>
        /// <param name="ChinaAreas">地区集合</param>
        /// <returns></returns>
        public void UpdateChinaArea(IEnumerable<ChinaArea> ChinaAreas)
        {
            _chinaAreaRepository.UpdateAsync(ChinaAreas);
        }

        #endregion
    }
}
