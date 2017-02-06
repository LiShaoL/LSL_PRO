using LSL_PRO.Kernel;
using LSL_PRO.Utilities;
using LSL_PRO_BLL;
using LSL_PRO_IBLL;
using LSL_PRO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSL_PRO_WEB.Controllers
{
    public class RegisterController : Controller
    {
        RegisterIBLL registerBLL = new RegisterBLL();
        ResultModel Rm = new ResultModel();
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterP()
        {
            string username = Request["name"];
            string pass = Request["pwd"];
            string Msg = string.Empty;
            
            LSL_PRO_User user = new LSL_PRO_User();
            user.UserId = CommonHelper.GetGuid;
            user.Account = username;
            user.Password = Md5Helper.MakeMd5(pass);
            registerBLL.AddUser(user, ref Rm);
            return Json(Rm);          
        }
        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegisterPU()
        {
            string username=Request["name"];
            registerBLL.isRegister(username, ref Rm);
            return Json(Rm);
        }
	}
}