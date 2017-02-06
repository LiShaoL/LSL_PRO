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
    public class LoginController : Controller
    {
        LoginIBLL loginIBLL = new LoginBLL();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginP()
        {
            string username = Request["username"];
            string userpass = Request["userpass"];
            SessionUser us=new SessionUser();
            ResultModel Rm = new ResultModel();
            if (loginIBLL.isLogin(username, userpass, ref Rm, ref us))
            {
                RequestSession.AddSessionUser(us);
                return Json(Rm);
            }
            else 
            {
                return Json(Rm);
            }
        }     
	}
}