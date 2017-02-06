using LSL_PRO.Kernel;
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
   public class LoginBLL:LoginIBLL
    {
       private readonly LoginIDAL dal = new LoginDAL();
        #region Method     
        /// <summary>  
        /// 根据字段得到一个对象实体  
        /// </summary>  
        /// <param name="Key">字段名</param>  
       /// <param name="Value">值</param>  
        /// <returns></returns>  
       public LSL_PRO_User GetEntity(string Key,string Value)
        {
            return dal.GetEntity(Key,Value);
        }
       /// <summary>
       /// 登录验证
       /// </summary>
       /// <param name="username">用户名</param>
       /// <param name="userpass">密码</param>
       /// <param name="Msg">消息</param>
       /// <param name="us">session信息</param>
       /// <returns></returns>
       public bool isLogin(string username, string userpass, ref ResultModel Msg, ref SessionUser us) 
       {
           bool result;
           LSL_PRO_User user = GetEntity("Account", username);
           if (!string.IsNullOrEmpty(user.Account))
           {
               if (user.Password == Md5Helper.MakeMd5(userpass))
               {
                   us.Account = user.Account;
                   us.Password = user.Password;
                   us.UserId = user.UserId;
                   Msg.code = "1";
                   Msg.msg = MessageHelper.MSG00;
                   result = true;
               }
               else 
               {
                   Msg.code = "-1";
                   Msg.msg = MessageHelper.MSG03;
                   result = false;
               }
               
           }
           else 
           {
               Msg.code = "-1";
               Msg.msg = MessageHelper.MSG04;
               result = false;
           }
           return result;
       }
        #endregion
    }
}
