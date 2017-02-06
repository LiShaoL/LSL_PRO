using LSL_PRO.Kernel;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IBLL
{
    public interface UserIBLL
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">页条数</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="sord">排序顺序</param>
        /// <returns></returns>
        IList GetList(StringBuilder sql, SqlParam[] sqlParam, int page, int rows, string sidx, string sord, ref int count);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Rm"></param>
        void AddUser(LSL_PRO_User user, ref ResultModel Rm);
        
        /// <summary>
        /// 根据UserID获取用户信息
        /// </summary>
        /// <param name="id">UserID</param>
        /// <param name="Rm"></param>
        void GetUserByID(string id, ref ResultModel Rm);

        void GetUserByAccount(string username, ref ResultModel Rm);

        void UpdateUser(LSL_PRO_User user, ref ResultModel Rm);

        void DeleteByID(string id, ref ResultModel Rm);
    }
}
