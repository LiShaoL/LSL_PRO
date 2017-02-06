using LSL_PRO.Kernel;
using LSL_PRO.Utilities;
using LSL_PRO_DAL;
using LSL_PRO_IBLL;
using LSL_PRO_IDAL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_BLL
{
    public class RolesPermissionBLL : RolesPermissionIBLL
    {
        RolesPermissionIDAL dal = new RolesPermissionDAL();
        /// <summary>
        /// 批量添加角色用户
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Rm"></param>
        public void InsertBySqlBulkCopy(DataTable dt,string tbname, ref ResultModel Rm)
        {
            if (dal.InsertBySqlBulkCopy(dt, tbname))
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG00;
            }
            else 
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
            }
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <returns></returns>
        public IList GetAddUserList(StringBuilder where, SqlParam[] param, int page, int rows, string sidx, string sord, ref int count, string roleid)
        {
            return dal.GetAddUserList(where, param, page, rows, sidx, sord, ref count,roleid);
        }
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
        public IList GetUpdateUserList(StringBuilder sql, SqlParam[] sqlParam, int page, int rows, string sidx, string sord, ref int count, string roleid)
        {
            return dal.GetUpdateUserList(sql, sqlParam, page, rows, sidx, sord, ref count, roleid);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="Rm"></param>
        public void DeleteUserByRole(string[] uids, ref ResultModel Rm)
        {
            if (dal.DeleteUserByRoleId(uids))
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG00;
            }
            else
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
            }
        }

        public IList GetJstree()
        {
            return dal.GetJstree();
        }

        public IList GetRoleMenu(string roleid)
        {
            return dal.GetRoleMenu(roleid);
        }
        public IList GetMenuJstree(string roleid)
        {
            return dal.GetMenuJstree(roleid);
        }
        public IList GetMenuBtn(string mid)
        {
            return dal.GetMenuBtn(mid);
        }

       public IList GetRoleMenuBtn(string roleid, string mid)
        {
            return dal.GetRoleMenuBtn(roleid,mid);
        }

       public bool DeleteRoleMenuButton(string roleid, string mid)
       {
           if (dal.DeleteMenuBtnByID(roleid,mid) >= 0)
           {
               return true;
           }
           else { return false; }
       }
       public bool DeleteRoleMenu(string roleid)
       {
           if (dal.DeleteRoleMenu(roleid) >= 0)
           {
               return true;
           }
           else { return false; }
       }
    }
}
