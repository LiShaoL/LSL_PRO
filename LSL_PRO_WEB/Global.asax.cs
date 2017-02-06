using LSL_PRO.Kernel;
using LSL_PRO_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LSL_PRO_WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //设置当前数据库类型
            DBHelperFactory.DbType = (SqlSourceType)Enum.Parse(typeof(SqlSourceType), ConfigHelper.GetValue("ComponentDbType"), true);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
