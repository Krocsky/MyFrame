using System.Web.Http;
using Service;
using System.Linq;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Autofac.Integration.WebApi;
using Common;
using WebApi.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    /// <summary>
    /// 地区
    /// </summary>
    [RoutePrefix("api/Area")]
    [AutofacControllerConfiguration]
    public class AreaController : ApiController
    {
        #region Ctor

        private readonly IChinaAreaService _chinaAreaService;
        private readonly UserService _userService;

        public AreaController(IOwinContext context, IChinaAreaService chinaAreaService)
        {
            this._userService = context.GetUserManager<UserService>();
            this._chinaAreaService = chinaAreaService;
        }

        #endregion

        /// <summary>
        /// 获取地区
        /// </summary>
        /// <returns></returns>
        [Route("GetAreas")]
        public IHttpActionResult GetAreas()
        {
            var areas = _chinaAreaService.ChinaAreas
                          .Select(s => new { AreaTypes = s.AreaType, Code = s.Code, Name = s.Name, ParentCode = s.ParentCode });
            return Ok(new MessageData(true, "success:)", areas));
        }

        /// <summary>
        /// 根据Id获取地区信息
        /// </summary>
        /// <param name="areaId">地区Id</param>
        /// <returns></returns>
        [Route("GetAreaById")]
        public IHttpActionResult GetAreaById(int areaId)
        {
            if (areaId == 0)
                return Ok(new MessageData(false, "找不到该地区"));
            return Ok(_chinaAreaService.FindChinaAreaById(areaId));
        }

        /// <summary>
        /// 根据地区编码获取地区信息
        /// </summary>
        /// <param name="code">地区编码</param>
        /// <returns></returns>
        [Route("GetAreaByCode")]
        public IHttpActionResult GetAreaByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Ok(new MessageData(false, "地区编码不能为空"));
            return Ok(_chinaAreaService.FindChinaAreaByCode(code));
        }
    }
}
