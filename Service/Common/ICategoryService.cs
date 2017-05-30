using Common;
using Common.Utilities;
using Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 分类业务逻辑接口
    /// </summary>
    public interface ICategoryService
    {
        IQueryable<Category> Categorys
        {
            get;
        }

        /// <summary>
        /// 根据Id获取分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category FindById(int id);

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="category"></param>
        void Create(Category category);

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="category"></param>
        void Update(Category category);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="category"></param>
        void Delete(Category category);

        /// <summary>
        /// 根据分类ID判断是否有子分类
        /// </summary>
        /// <param name="id"></param>
        bool HasChildren(int id);

        /// <summary>
        /// 根据ID判断在物资设置中是否被使用
        /// </summary>
        /// <param name="id"></param>
        //bool InUse(int id);

        /// <summary>
        /// 检查学校下分类名称是否存在
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="name"></param>
        bool NameIsExists(int schoolId, string name);

        /// <summary>
        /// 获取学校下的所有分类
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        IQueryable<Category> GetAllCategories(int schoolId); 
    }
}
