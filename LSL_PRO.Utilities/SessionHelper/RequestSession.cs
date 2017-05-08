using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// Session 帮助类
    /// </summary>
    public class RequestSession
    {
        public RequestSession()
        {

        }
        private static string SESSION_USER = "SESSION_USER";
        public static void AddSessionUser(SessionUser user)
        {
            HttpContext rq = HttpContext.Current;
            rq.Session[SESSION_USER] = user;
        }
        public static SessionUser GetSessionUser()
        {
            HttpContext rq = HttpContext.Current;
            SessionUser user = new SessionUser();

            if (rq != null && rq.Session != null)
                user = (SessionUser)rq.Session[SESSION_USER];
            return user;
        }
    }
}