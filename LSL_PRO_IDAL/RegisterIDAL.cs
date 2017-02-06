using LSL_PRO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IDAL
{
   public interface RegisterIDAL
    {
        int AddUser(LSL_PRO_User user);

        LSL_PRO_User GetEntity(string Key, string Value);
    }
}
