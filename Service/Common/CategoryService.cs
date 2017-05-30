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
    /// 分类业务逻辑
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public virtual IQueryable<Category> Categorys
        {
            get
            {
                return _categoryRepository.Table;
            }
        }

        /// <summary>
        /// 根据Id获取分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category FindById(int id)
        {
            return _categoryRepository.FirstOrDefault(id);
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="category"></param>
        public void Create(Category category)
        {
            _categoryRepository.Insert(category);
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="category"></param>
        public void Update(Category category)
        {
            _categoryRepository.Update(category);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="category"></param>
        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }

        /// <summary>
        /// 根据分类ID判断是否有子分类
        /// </summary>
        /// <param name="id"></param>
        public bool HasChildren(int id)
        {
            if (_categoryRepository.Table.Count(n => n.ParentId == id) == 0)
                return false;
            return true;
        }

        ///// <summary>
        ///// 根据ID判断在物资设置中是否被使用
        ///// </summary>
        ///// <param name="id"></param>
        //public bool InUse(int id)
        //{
        //    var schoolService = DIContainer.Resolve<ISchoolService>();
        //    if (schoolService.ItemStocks.Count(n => n.CategoryId == id) == 0)
        //        return false;
        //    return true;
        //}

        /// <summary>
        /// 检查分类名称是否存在
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="name"></param>
        public bool NameIsExists(int schoolId, string name)
        {
            if (_categoryRepository.Table.Where(n => n.SchoolId == schoolId).ToList().FirstOrDefault(n => n.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;
            return true;
        }

        /// <summary>
        /// 获取学校下的所有分类
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        public IQueryable<Category> GetAllCategories(int schoolId)
        {
            return _categoryRepository.Table.Where(n => n.SchoolId == schoolId);
        }
    }
}
