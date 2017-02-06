using LSL_PRO_IDAL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_DAL
{
    public class IndexDAL:IndexIDAL
    {
        public IList GetList(string id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select M.* FROM dbo.LSL_PRO_User U LEFT JOIN dbo.LSL_PRO_UserRole UR ON U.UserId = UR.UserId  LEFT JOIN dbo.LSL_PRO_RoleMenu RM ON UR.RoleId = RM.RoleId LEFT JOIN dbo.LSL_PRO_Menu M ON M.MenuId=RM.MenuId WHERE U.UserId='"+id+"'");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<LSL_PRO_Menu>(sql,null);
        }
    }
}
