using LSL_PRO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LSL_PRO_WEB
{
    /// <summary>
    /// 重定向到登录页面
    /// </summary>
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            {
                SessionUser user=RequestSession.GetSessionUser();
                if ( user==null||user.Account== null)
                    filterContext.HttpContext.Response.Redirect("/Login/Login");
                base.OnActionExecuting(filterContext);
            }
        }
    }
}