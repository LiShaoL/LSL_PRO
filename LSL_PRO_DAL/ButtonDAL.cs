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
    public class ButtonDAL : ButtonIDAL
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
            sql.Append("select * from LSL_PRO_Button where 1=1");
            sql.Append(where);
            return DBHelperFactory.SqlHelper().GetPageList<LSL_PRO_Button>(sql.ToString(), param, sidx, sord, page, rows, ref count);
        }
        /// <summary>
        ///增加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int AddButton(LSL_PRO_Button user)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Insert(user);
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdateBtn(LSL_PRO_Button user)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Update(user, "ButtonId");
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public LSL_PRO_Button GetEntity(string Key, string Value)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).GetModelById<LSL_PRO_Button>(Key, Value);
        }
        public int DeleteById(string id)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Delete("LSL_PRO_Button", "ButtonId", id);
        }
    }
}
