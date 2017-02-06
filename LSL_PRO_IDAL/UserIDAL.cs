using LSL_PRO.Kernel;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IDAL
{
    public interface UserIDAL
    {
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <returns></returns>
        IList GetList(StringBuilder where, SqlParam[] param, int page, int rows, string sidx, string sord, ref int count);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int AddUser(LSL_PRO_User user);
        /// <summary>
        /// 根据字段获取数据
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        LSL_PRO_User GetEntity(string Key, string Value);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int UpdateUser(LSL_PRO_User user);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteById(string id);
    }
}
