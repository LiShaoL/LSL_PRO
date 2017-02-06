using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_Model
{
   public class LSL_PRO_UserRole
    {
        public string UserRoleId { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
    }
}
