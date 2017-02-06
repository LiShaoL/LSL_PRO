using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_Model
{
   public class LSL_PRO_UserRoleInfo
    {
        public string UserRoleId { get; set; }
        public string UserId { get; set; }
        public string Account { get; set; }
        public string RealName { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }

    }
}
