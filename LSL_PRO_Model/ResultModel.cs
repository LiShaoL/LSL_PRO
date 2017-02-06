using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_Model
{
   public class ResultModel
    {
        public string code
        {
            get;
            set;
        }

        public string msg
        {
            get;
            set;
        }

        public object result
        {
            get;
            set;
        }

        public ResultModel()
        {
            this.code = "-1";
            this.msg = "";
            this.result = "[]";
        }
    }
}
