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
    /// 模板业务逻辑
    /// </summary>
    public class TemplateService : ITemplateService
    {
        private readonly IRepository<Template> _templateRepository;

        public TemplateService(IRepository<Template> templateRepository)
        {
            this._templateRepository = templateRepository;
        }

        public virtual IQueryable<Template> Templates
        {
            get
            {
                return _templateRepository.Table;
            }
        }

        /// <summary>
        /// 根据Id获取模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Template FindById(int id)
        {
            return _templateRepository.FirstOrDefault(id);
        }

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="template"></param>
        public void Create(Template template)
        {
            _templateRepository.Insert(template);
        }

        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="template"></param>
        public void Update(Template template)
        {
            _templateRepository.Update(template);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="template"></param>
        public void Delete(Template template)
        {
            _templateRepository.Delete(template);
        }
    }
}
