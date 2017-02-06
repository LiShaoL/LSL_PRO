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
    public class MenuBLL : MenuIBLL
    {
        MenuIDAL dal = new MenuDAL();
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
            return dal.GetList(where, param, page, rows, sidx, sord, ref count);
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <param name="Rm">结果实体</param>
        /// <returns></returns>
        public void Add(LSL_PRO_Menu user, ref ResultModel Rm)
        {
            if (dal.Add(user) >= 0)
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG05;
            }
            else
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
            }
        }
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Rm"></param>
        public void Update(LSL_PRO_Menu user, ref ResultModel Rm)
        {
            if (dal.Update(user) >= 0)
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG06;
            }
            else
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
            }
        }
        /// <summary>
        /// 根据字段获取数据
        /// </summary>
        /// <param name="Key">字段名</param>
        /// <param name="Value">字段值</param>
        /// <returns></returns>
        public LSL_PRO_Menu GetEntity(string Key, string Value)
        {
            return dal.GetEntity(Key, Value);
        }
        /// <summary>
        /// 根据ID获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rm"></param>
        public void GetMenuByID(string id, ref ResultModel Rm)
        {
            LSL_PRO_Menu menu = GetEntity("MenuId", id);
            if (!string.IsNullOrEmpty(menu.MenuId))
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG00;
                Rm.result = menu;
            }
            else
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
                Rm.result = menu;
            }
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rm"></param>
        public void DeleteByID(string id, ref ResultModel Rm)
        {
            if (dal.DeleteById(id) >= 0)
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG07;
            }
            else
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
            }
        }
        /// <summary>
        /// 获取上级菜单
        /// </summary>
        /// <returns></returns>
        public IList GetTopMune()
        {
            return dal.GetTopMune();
        }
        /// <summary>
        /// 批量添加按钮
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtname"></param>
        /// <param name="Rm"></param>
        public void AddMenuBtn(DataTable dt, string dtname, ref ResultModel Rm)
        {
            if (dal.AddMenuBtns(dt, dtname))
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
        /// 获取已存在的菜单按钮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetMenuBtn(string id)
        {
            return dal.GetMenuBtn(id);
        }
        public bool DeleteMenuBtnByID(string menuID)
        {
            if (dal.DeleteMenuBtnByID(menuID) >= 0)
            {
                return true;
            }
            else { return false; }
        }
    }
}
