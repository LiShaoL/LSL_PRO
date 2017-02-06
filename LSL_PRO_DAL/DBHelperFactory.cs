using LSL_PRO.DataAccess;
using LSL_PRO.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_DAL
{
   public class DBHelperFactory
    {
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        public static SqlSourceType DbType { get; set; }

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbHelper SqlHelper()
        {
            return new SqlServerHelper(ConfigHelper.GetValue("SqlServer_LSL"), DbType.ToString());
        }       
        /// <summary>
        /// 公共方法操作 增、删、改、查
        /// </summary>
        /// <param name="connectionString">链接字符串</param>
        /// <returns></returns>
        public static IDbUtilities DbUtils(string connectionString)
        {          
                return new DbUtilities(connectionString, DbType);  
        }
    }
}
