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
    public interface ButtonIBLL
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
        void AddButton(LSL_PRO_Button user, ref ResultModel Rm);

        void UpdateBtn(LSL_PRO_Button user, ref ResultModel Rm);

        void DeleteByID(string id, ref ResultModel Rm);

        void GetButtonByID(string id, ref ResultModel Rm);
    }
}
