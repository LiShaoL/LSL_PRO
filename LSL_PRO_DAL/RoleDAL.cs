using LSL_PRO.Kernel;
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
    public class RoleDAL : RoleIDAL
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <returns></returns>
        public IList GetList(StringBuilder where, SqlParam[] param, int page, int rows, string sidx, string sord, ref int count)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from LSL_PRO_Roles where 1=1");
            sql.Append(where);
            return DBHelperFactory.SqlHelper().GetPageList<LSL_PRO_Roles>(sql.ToString(), param, sidx, sord, page, rows, ref count);
        }
        /// <summary>
        ///增加角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Add(LSL_PRO_Roles user)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Insert(user);
        }
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(LSL_PRO_Roles user)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Update(user, "RoleId");
        }

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public LSL_PRO_Roles GetEntity(string Key, string Value)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).GetModelById<LSL_PRO_Roles>(Key, Value);
        }
        public int DeleteById(string id)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Delete("LSL_PRO_Roles", "RoleId", id);
        }
    }
}
