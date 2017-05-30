using Common;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Service;
using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        public readonly UserService _userService;
        public readonly RoleService _roleService;

        public ApiController(IOwinContext context)
        {
            this._userService = context.GetUserManager<UserService>();
            this._roleService = context.GetUserManager<RoleService>();
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file_data, UserType tenantType, int itemId = 0)
        {
            if (file_data == null)
                return Json(new MessageData(false, "上传失败，请检查要上传的文件是否存在！"));
            Attachment attachment = new Attachment(file_data);
            attachment.UserId = UserContext.CurrentUser.Id;
            attachment.TenantType = tenantType;
            attachment.ItemId = itemId;
            DIContainer.Resolve<IAttachmentService>().Create(attachment, file_data.InputStream);

            return Json(new MessageData(true, "上传成功"));
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAvatar()
        {
            AvatarResult result = new AvatarResult();
            result.avatarUrls = new ArrayList();
            result.success = false;
            result.msg = "Failure!";

            int userId = UserContext.CurrentUserId;
            if (userId == 0)
                return Json(result);

            #region 处理原始图片

            HttpPostedFileBase file = Request.Files["__source"];
            if (file != null)
            {
                string sourceFileName = file.FileName;
                //原始文件的扩展名
                //string sourceExtendName = Path.GetExtension(sourceFileName);
                //基于原图的初始化参数
                string initParams = Request.Form["__initParams"];
                result.sourceUrl = string.Format("~/Upload/Avatar/{0}_original.jpg", userId);
                file.SaveAs(Server.MapPath(result.sourceUrl));
                result.sourceUrl += initParams;
                //保存到数据库
            }

            #endregion

            #region 处理头像图片

            //默认的 file 域名称：__avatar1,2,3... 参数名：avatar_field_names
            var avatars = Request.Files.AllKeys.Where(n => n.Contains("__avatar"));

            foreach (var avatar in avatars)
            {
                file = Request.Files[avatar];
                string virtualPath = string.Format("~/Upload/Avatar/{0}.jpg", userId);
                result.avatarUrls.Add(virtualPath);
                file.SaveAs(Server.MapPath(virtualPath));
            }

            #endregion

            result.success = true;
            result.msg = "上传成功";
            return Json(result);
        }

        #region 删除附件

        public ActionResult DeleteAttachment(int id)
        {
            var attachment = DIContainer.Resolve<IAttachmentService>().FindById(id);
            if (attachment == null)
                return Json(new MessageData(false, "附件不存在"));
            try
            {
                DIContainer.Resolve<IAttachmentService>().Delete(attachment);
                return Json(new MessageData(true, "删除成功"));
            }
            catch (Exception)
            {
                return Json(new MessageData(false, "删除失败"));
            }
        }

        #endregion
    }
}