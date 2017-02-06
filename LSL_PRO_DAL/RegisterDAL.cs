using LSL_PRO.Kernel;
using LSL_PRO_IDAL;
using LSL_PRO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_DAL
{
    public class RegisterDAL : RegisterIDAL
    {
        public int AddUser(LSL_PRO_User user)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).Insert(user);
        }
        public LSL_PRO_User GetEntity(string Key, string Value)
        {
            return DBHelperFactory.DbUtils(ConfigHelper.GetValue("SqlServer_LSL")).GetModelById<LSL_PRO_User>(Key, Value);
        }
    }
}
