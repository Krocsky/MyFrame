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
    /// 模板业务逻辑接口
    /// </summary>
    public interface ITemplateService
    {
        IQueryable<Template> Templates
        {
            get;
        }

        /// <summary>
        /// 根据Id获取模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Template FindById(int id);

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="template"></param>
        void Create(Template template);

        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="template"></param>
        void Update(Template template);

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="template"></param>
        void Delete(Template template);
    }
}
