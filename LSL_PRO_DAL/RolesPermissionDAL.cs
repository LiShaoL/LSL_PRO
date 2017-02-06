using LSL_PRO.Kernel;
using LSL_PRO_IDAL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_DAL
{
    public class RolesPermissionDAL : RolesPermissionIDAL
    {
        /// <summary>
        /// 批量添加角色用户
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertBySqlBulkCopy(DataTable dt, string tbname)
        {
            return DBHelperFactory.SqlHelper().BulkInsert(dt, tbname);
        }
        /// <summary>
        ///  分页获取添加用户数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParam">参数</param>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <param name="count">总数</param>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public IList GetAddUserList(StringBuilder where, SqlParam[] param, int page, int rows, string sidx, string sord, ref int count, string roleid)
        {
            StringBuilder sql = new StringBuilder();
            if (isRole(roleid))
            {
                sql.Append("SELECT U.UserId,U.Account,U.RealName,U.Gender,U.Mobile,U.Email,U.CreateDate FROM (SELECT * FROM dbo.LSL_PRO_UserRole WHERE RoleId='" + roleid + "') R  right JOIN dbo.LSL_PRO_User U  ON U.UserId = R.UserId WHERE R.UserRoleId IS NULL");
            }
            else
            {
                sql.Append("select * from LSL_PRO_User where 1=1");
            }
            sql.Append(where);
            return DBHelperFactory.SqlHelper().GetPageList<LSL_PRO_User>(sql.ToString(), param, sidx, sord, page, rows, ref count);
        }
        /// <summary>
        /// 判断角色是否存在用户
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        private bool isRole(string roleid)
        {
            LSL_PRO_UserRole st = DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).GetModelById<LSL_PRO_UserRole>("RoleId", roleid);
            if (!string.IsNullOrEmpty(st.UserRoleId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///  分页获取编辑用户数据
        /// </summary>
        /// <param name="where">sql语句</param>
        /// <param name="sqlParam">参数</param>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <param name="count">总数</param>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public IList GetUpdateUserList(StringBuilder where, LSL_PRO.Kernel.SqlParam[] sqlParam, int page, int rows, string sidx, string sord, ref int count, string roleid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT R.UserRoleId,U.UserId,U.Account,U.RealName,U.Gender,U.Mobile,U.Email,U.CreateDate FROM(SELECT * FROM dbo.LSL_PRO_UserRole WHERE RoleId='" + roleid + "') R LEFT JOIN dbo.LSL_PRO_User U ON R.UserId = U.UserId");
            sql.Append(where);
            return DBHelperFactory.SqlHelper().GetPageList<LSL_PRO_UserRoleInfo>(sql.ToString(), sqlParam, sidx, sord, page, rows, ref count);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        public bool DeleteUserByRoleId(string[] uids)
        {
            int i = DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).BatchDelete("LSL_PRO_UserRole", "UserRoleId", uids);
            bool a = (i >= 0) ? true : false;
            return a;
        }
        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        /// <returns></returns>
        public IList GetJstree()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT M.MenuId AS id,M.ParentId AS parent,M.FullName AS text,'fa '+M.Img AS icon FROM dbo.LSL_PRO_Menu M");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<JstreeModel>(sql, null);
        }

        public IList GetRoleMenu(string roleid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT MenuId FROM  dbo.LSL_PRO_RoleMenu WHERE RoleId='" + roleid + "'");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<LSL_PRO_RoleMenu>(sql, null);
        }
        public IList GetMenuJstree(string roleid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT M.MenuId AS id,M.ParentId AS parent,M.FullName AS text,'fa '+M.Img AS icon FROM dbo.LSL_PRO_RoleMenu R LEFT JOIN dbo.LSL_PRO_Menu M ON R.MenuId = M.MenuId WHERE R.RoleId='" + roleid + "'");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<JstreeModel>(sql, null);
        }
        public IList GetMenuBtn(string mid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT M.ButtonId,B.FullName FROM dbo.LSL_PRO_MenuButton M LEFT JOIN dbo.LSL_PRO_Button B ON M.ButtonId = B.ButtonId  WHERE MenuId='" + mid + "'");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<LSL_PRO_Button>(sql, null);
        }
        public IList GetRoleMenuBtn(string roleid, string mid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM dbo.LSL_PRO_RoleMenuButton WHERE RoleId='" + roleid + "' AND MenuId='" + mid + "'");
            return DBHelperFactory.SqlHelper().GetDataListBySQL<LSL_PRO_RoleMenuButton>(sql, null);
        }

        public int DeleteMenuBtnByID(string roleid, string mid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE dbo.LSL_PRO_RoleMenuButton WHERE RoleId='" + roleid + "' AND MenuId='" + mid + "'");
            return DBHelperFactory.SqlHelper().ExecuteBySql(sql);
        }

        public int DeleteRoleMenu(string roleid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE dbo.LSL_PRO_RoleMenu WHERE RoleId='" + roleid + "'");
            return DBHelperFactory.SqlHelper().ExecuteBySql(sql);
        }
    }
}
