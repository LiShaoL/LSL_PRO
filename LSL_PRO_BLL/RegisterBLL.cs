using LSL_PRO.Utilities;
using LSL_PRO_DAL;
using LSL_PRO_IBLL;
using LSL_PRO_IDAL;
using LSL_PRO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_BLL
{
    public class RegisterBLL : RegisterIBLL
    {
        RegisterIDAL dal = new RegisterDAL();
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <param name="Rm">结果实体</param>
        /// <returns></returns>
        public void AddUser(LSL_PRO_User user, ref ResultModel Rm) 
        {
            if (dal.AddUser(user) >= 0)
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG13;
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
        public LSL_PRO_User GetEntity(string Key, string Value)
        {
            return dal.GetEntity(Key, Value);
        }
        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Rm"></param>
        public void isRegister(string name,ref ResultModel Rm)
        {           
            LSL_PRO_User user = GetEntity("Account", name);
            if (!string.IsNullOrEmpty(user.Account))
            {
                Rm.code = "-1";
                Rm.msg = MessageHelper.MSG15;
            }
            else
            {
                Rm.code = "1";
                Rm.msg = MessageHelper.MSG14;               
            }
            
        }
    }
}
