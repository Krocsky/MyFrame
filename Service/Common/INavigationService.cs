using Common;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 导航业务逻辑接口
    /// </summary>
    public interface INavigationService
    {
        IQueryable<Navigation> Navigations
        {
            get;
        }

        /// <summary>
        /// 根据Id获取导航
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Navigation FindById(int id);

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="navigation"></param>
        void Create(Navigation navigation);

        /// <summary>
        /// 更新导航
        /// </summary>
        /// <param name="navigation"></param>
        void Update(Navigation navigation);

        /// <summary>
        /// 删除导航
        /// </summary>
        /// <param name="navigation"></param>
        void Delete(Navigation navigation);

        /// <summary>
        /// 获取校区下的根导航
        /// </summary>
        /// <returns></returns>
        IList<Navigation> GetRootNavigations();

        /// <summary>
        /// 清空学校导航
        /// </summary>
        void ClearNavigations();

        /// <summary>
        /// 设置学校菜单权限
        /// </summary>
        /// <param name="navigationIds"></param>
        void SetSchoolNavigations(IEnumerable<int> navigationIds);

        /// <summary>
        /// 设置用户菜单权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="navigationIds"></param>
        void SetUserNavigations(int userId, IEnumerable<int> navigationIds);

        /// <summary>
        /// 判断菜单名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsExists(string name);
    }
}
