using LSL_PRO.Kernel;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IDAL
{
    public interface MenuIDAL
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
        int Add(LSL_PRO_Menu user);
        /// <summary>
        /// 根据字段获取数据
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        LSL_PRO_Menu GetEntity(string Key, string Value);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int Update(LSL_PRO_Menu user);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteById(string id);
        /// <summary>
        /// 获取顶层菜单
        /// </summary>
        /// <returns></returns>
        IList GetTopMune();

        /// <summary>
        /// 批量添加按钮
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtname"></param>
        /// <returns></returns>
        bool AddMenuBtns(DataTable dt, string dtname);

        IList GetMenuBtn(string id);

        int DeleteMenuBtnByID(string menuID);
    }
}
