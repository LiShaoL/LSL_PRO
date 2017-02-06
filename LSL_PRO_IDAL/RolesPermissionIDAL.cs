using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IDAL
{
    public interface RolesPermissionIDAL
    {
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool InsertBySqlBulkCopy(DataTable dt,string tbname);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="where">sql</param>
        /// <param name="param">参数</param>
        /// <param name="page">页</param>
        /// <param name="rows">页数</param>
        /// <param name="sidx">排序</param>
        /// <param name="sord">排序顺序</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        IList GetAddUserList(StringBuilder where, LSL_PRO.Kernel.SqlParam[] param, int page, int rows, string sidx, string sord, ref int count, string roleid);
        /// <summary>
        ///  分页获取编辑用户数据
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
        IList GetUpdateUserList(StringBuilder sql, LSL_PRO.Kernel.SqlParam[] sqlParam, int page, int rows, string sidx, string sord, ref int count, string roleid);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        bool DeleteUserByRoleId(string[] uids);

        IList GetJstree();

        IList GetRoleMenu(string roleid);

        IList GetMenuJstree(string roleid);

        IList GetMenuBtn(string mid);

        IList GetRoleMenuBtn(string roleid, string mid);

        int DeleteMenuBtnByID(string roleid, string mid);

        int DeleteRoleMenu(string roleid);
    }
}
