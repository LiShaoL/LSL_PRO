
using LSL_PRO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IBLL
{
    public interface RegisterIBLL
    {
        void AddUser( LSL_PRO_User user, ref ResultModel Rm);

        LSL_PRO_User GetEntity(string Key, string Value);

        void isRegister(string username, ref ResultModel Rm);
    }
}
