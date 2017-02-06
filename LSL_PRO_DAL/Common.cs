using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_DAL
{
   public class Common
    {
        //根据权限获取按钮
        public static IList GetBtnByRole(string uid,string mid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT B.* FROM dbo.LSL_PRO_User U LEFT JOIN dbo.LSL_PRO_UserRole UR ON U.UserId = UR.UserId LEFT JOIN dbo.LSL_PRO_RoleMenuButton RM ON UR.RoleId = RM.RoleId LEFT JOIN dbo.LSL_PRO_Button B ON RM.ButtonId = B.ButtonId WHERE U.UserId='" + uid + "' AND MenuId ='" + mid + "' ORDER BY B.CreateDate");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<LSL_PRO_Button>(sql, null);
        }
    }
}
