using LSL_PRO_DAL;
using LSL_PRO_IBLL;
using LSL_PRO_IDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_BLL
{
    public class IndexBLL : IndexIBLL
    {
        IndexIDAL dal = new IndexDAL();
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public IList GetList(string id) 
        {
            return dal.GetList(id);
        }
    }
}
