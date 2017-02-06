using LSL_PRO.Kernel;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IBLL
{
    public interface RolesPermissionIBLL
    {
        /// <summary>
        /// 批量添加角色用户
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="Rm">信息</param>
        void InsertBySqlBulkCopy(DataTable dt,string tbname, ref ResultModel Rm);

        /// <summary>
        /// 分页获取添加用户数据
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <param name="count">总数</param>
        /// <returns></returns>
        IList GetAddUserList(StringBuilder sql, SqlParam[] sqlParam, int page, int rows, string sidx, string sord, ref int count,string roleid);
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

        IList GetUpdateUserList(StringBuilder sql, SqlParam[] sqlParam, int page, int rows, string sidx, string sord, ref int count, string roleid);
        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="uids"></param>
        void DeleteUserByRole(string[] uids, ref ResultModel Rm);

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        /// <returns></returns>
        IList GetJstree();

        IList GetRoleMenu(string roleid);

        IList GetMenuJstree(string roleid);

        IList GetMenuBtn(string mid);

        IList GetRoleMenuBtn(string roleid,string mid);

        bool DeleteRoleMenuButton(string roleid, string mid);

        bool DeleteRoleMenu(string roleid);
    }
}
