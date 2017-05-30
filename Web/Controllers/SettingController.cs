using Common;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Service;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        #region Ctor

        public readonly UserService _userService;
        public readonly RoleService _roleService;
        public readonly ISettingService _settingService;
        public readonly ILogger _loggerService;

        public SettingController(IOwinContext context, ISettingService settingService, ILogger loggerService)
        {
            this._userService = context.GetUserManager<UserService>();
            this._roleService = context.GetUserManager<RoleService>();
            this._settingService = settingService;
            this._loggerService = loggerService;
        }

        #endregion

        #region 设置

        /// <summary>
        /// 设置管理
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">一页的显示条数</param>
        /// <returns></returns>
        public ActionResult ManageSetting(string searchInputs = null, int pageIndex = 1, int pageSize = 20)
        {
            var list = new PagedList<Setting>(_settingService.Settings.OrderByDescending(n => n.Id)
                        .WhereIf(!String.IsNullOrEmpty(searchInputs), n => n.Name.ToLower().Contains(searchInputs.Trim().ToLower()) || n.Code.ToLower().Contains(searchInputs.Trim().ToLower())), pageIndex, pageSize);
            return View(list);
        }

        /// <summary>
        /// 编辑设置
        /// </summary>
        /// <param name="id">设置Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditSetting(int? id = null)
        {
            Setting Setting;
            if (id == null)
                Setting = new Setting { IsDelete = false, CreatedTime = DateTime.Now };
            else
            {
                Setting = _settingService.FindSettingById(id.Value);
            }

            if (Setting == null)
                return HttpNotFound();

            return View(Setting.MapTo<SettingEditModel>());
        }

        /// <summary>
        /// 编辑设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSetting(SettingEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new MessageData(false, "操作失败"));
            }
            Setting Setting;
            if (model.Id > 0)
            {
                Setting = _settingService.FindSettingById(model.Id);
                if (Setting == null)
                    return Json(new MessageData(false, "设置不存在"));
                var nameExists = _settingService.Settings.FirstOrDefault(n => n.Id != model.Id && n.Name == model.Name) != null;
                if (nameExists)
                    return Json(new MessageData(false, "设置已存在"));
                Setting.Name = model.Name;
                Setting.Code = model.Code;
                Setting.Number = model.Number;
                try
                {
                    _settingService.UpdateSetting(Setting);
                    return Json(new MessageData(true, "编辑成功"));
                }
                catch (Exception ex)
                {
                    _loggerService.Error(ex);
                    return Json(new MessageData(false, "编辑失败，请重新编辑"));
                }
            }
            else
            {
                if (_settingService.IsExists(model.Name))
                {
                    return Json(new MessageData(false, "名称已存在"));
                }
                Setting = new Setting()
                {
                    Name = model.Name,
                    Code = model.Code,
                    Number = model.Number
                };
                try
                {
                    _settingService.InsertSetting(Setting);
                    return Json(new MessageData(true, "添加成功"));
                }
                catch (Exception ex)
                {
                    _loggerService.Error(ex);
                    return Json(new MessageData(false, "添加失败"));
                }
            }
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="id">设置id</param>
        /// <returns></returns>
        public ActionResult DeleteSetting(int id)
        {
            var Setting = _settingService.FindSettingById(id);
            if (Setting == null)
                return Json(new MessageData(false, "设置不存在"));
            Setting.IsDelete = true;
            try
            {
                _settingService.UpdateSetting(Setting);
                return Json(new MessageData(true, "删除成功"));
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex);
                return Json(new MessageData(false, "删除失败"));
            }
        }

        #endregion
    }
}