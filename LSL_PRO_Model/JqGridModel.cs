using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO_Model
{
   public class JqGridModel
    {
       /// <summary>
       /// 
       /// </summary>
        /// <param name="page1">当前页数</param>
       /// <param name="rows">每页条数</param>
       /// <param name="total">总数</param>
       /// <param name="objects">数据集合</param>
       /// <returns></returns>
       public static object GridData(int page1, int rows, int total, IList objects)
       {
           int pageSize = rows;
           var totalPages = (int)Math.Ceiling((float)total / pageSize);

           var jsonData = new
           {
               total = totalPages,
               page = page1,
               records = total,
               rows = objects
           };

           return jsonData;
       }
    }
}
