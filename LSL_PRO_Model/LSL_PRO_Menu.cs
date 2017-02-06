using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_Model
{
   public class LSL_PRO_Menu
    {
        public string MenuId { get; set; }
        public string ParentId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public string NavigateUrl { get; set; }
        public Nullable<int> AllowEdit { get; set; }
        public Nullable<int> AllowDelete { get; set; }
        public Nullable<int> Enabled { get; set; }
        public Nullable<int> SortCode { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyUserId { get; set; }
        public string ModifyUserName { get; set; }
    }
}
