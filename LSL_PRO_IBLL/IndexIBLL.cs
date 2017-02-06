using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_IBLL
{
   public interface IndexIBLL
    {
       /// <summary>
       /// 获取数据列表
       /// </summary>
       /// <returns></returns>
        IList GetList(string id);
    }
}
