using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_Model
{
   public class LSL_PRO_User
    {
        public string UserId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Alias { get; set; }
        public string RoleId { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public string Email { get; set; }
        public string OICQ { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> ChangePasswordDate { get; set; }
        public string IPAddress { get; set; }
        public Nullable<DateTime> FirstVisit { get; set; }
        public Nullable<DateTime> PreviousVisit { get; set; }
        public Nullable<DateTime> LastVisit { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public Nullable<DateTime> ModifyDate { get; set; }
        public string ModifyUserId { get; set; }
        public string ModifyUserName { get; set; }
        public string UserType { get; set; }
        public Nullable<int> IsLogin { get; set; }
    }
}
