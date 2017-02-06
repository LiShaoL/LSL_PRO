using LSL_PRO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IDAL
{
   public interface LoginIDAL
    {
        #region Method
        /// <summary>  
        /// 得到一个对象实体  
        /// </summary>  
        /// <param name="KeyValue">主键</param>  
        /// <returns></returns>  
       LSL_PRO_User GetEntity(string Key, string Value);
        #endregion
    }
}
