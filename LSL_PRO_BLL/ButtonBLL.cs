using LSL_PRO.Kernel;
using LSL_PRO.Utilities;
using LSL_PRO_DAL;
using LSL_PRO_IBLL;
using LSL_PRO_IDAL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_BLL
{
    public class ButtonBLL : ButtonIBLL
    {
        ButtonIDAL dal = new ButtonDAL();
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
        /// 添加按钮
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <param name="Rm">结果实体</param>
        /// <returns></returns>
        public void AddButton(LSL_PRO_Button user, ref ResultModel Rm)
        {
            if (dal.AddButton(user) >= 0)
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
        /// 修改按钮信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Rm"></param>
        public void UpdateBtn(LSL_PRO_Button user, ref ResultModel Rm)
        {
            if (dal.UpdateBtn(user) >= 0)
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
        public LSL_PRO_Button GetEntity(string Key, string Value)
        {
            return dal.GetEntity(Key, Value);
        }
        /// <summary>
        /// 根据ID获取按钮信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rm"></param>
        public void GetButtonByID(string id, ref ResultModel Rm)
        {
            LSL_PRO_Button btn = GetEntity("ButtonId", id);
            if (!string.IsNullOrEmpty(btn.ButtonId))
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG00;
                Rm.result = btn;
            }
            else
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG01;
                Rm.result = btn;
            }
        }
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
    }
}
