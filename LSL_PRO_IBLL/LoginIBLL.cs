using LSL_PRO.Kernel;
using LSL_PRO.Utilities;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IBLL
{
   public interface LoginIBLL
    {
        #region Method      
        /// <summary>  
        /// 根据字段得到一个对象实体  
        /// </summary>  
        /// <param name="Key">键</param>  
        /// <param name="Value">值</param> 
        /// <returns></returns>  
        LSL_PRO_User GetEntity(string Key,string Value);
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="userpass">密码</param>
        /// <param name="Msg">消息</param>
        /// <param name="us">session信息</param>
        /// <returns></returns>
        bool isLogin(string username, string userpass, ref ResultModel Msg, ref SessionUser us);
        #endregion
        
    }
}
