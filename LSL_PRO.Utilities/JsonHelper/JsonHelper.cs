 
// 
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Collections;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 转换Json格式帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 创建系统异常日志
        /// </summary>
        protected static LogHelper Logger = new LogHelper("转换Json格式帮助类");
        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(T t)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string json = "";
                if (t != null)
                {
                    sb.Append("{");
                    PropertyInfo[] properties = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in properties)
                    {
                        sb.Append("\"" + pi.Name.ToString() + "\"");
                        sb.Append(":");
                        sb.Append("\"" + pi.GetValue(t, null) + "\"");
                        sb.Append(",");
                    }
                    json = sb.ToString().TrimEnd(',');
                    json += "}";
                }
                return json;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// IList转Json
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string DropToJson<T>(IList list)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        T obj = Activator.CreateInstance<T>();
                        PropertyInfo[] pi = obj.GetType().GetProperties();
                        sb.Append("{");
                        for (int j = 0; j < pi.Length; j++)
                        {
                            sb.Append("\"");
                            sb.Append(pi[j].Name.ToString());
                            sb.Append("\":\"");
                            if (pi[j].GetValue(list[i], null) != null && pi[j].GetValue(list[i], null) != DBNull.Value && pi[j].GetValue(list[i], null).ToString() != "")
                            {
                                sb.Append(pi[j].GetValue(list[i], null)).Replace("\\", "/");
                            }
                            else
                            {
                                sb.Append("");
                            }
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// DataTable转Json
        /// </summary>
        /// <param name="dt">table数据集</param>
        /// <param name="dtName">json名</param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt, string dtName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{\"");
                sb.Append(dtName);
                sb.Append("\":[");
                if (DataTableHelper.IsExistRows(dt))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("{");
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            sb.Append("\"");
                            sb.Append(dc.ColumnName);
                            sb.Append("\":\"");
                            if (dr[dc] != null && dr[dc] != DBNull.Value && dr[dc].ToString() != "")
                                sb.Append(dr[dc]).Replace("\\", "/");
                            else
                                sb.Append("");
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]}");
                return JsonCharFilter(sb.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// IList转Json
        /// </summary>
        /// <param name="dt">IList</param>
        /// <param name="dtName">json名</param>
        /// <returns></returns>
        public static string ListToJson<T>(IList list, string dtName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{\"");
                sb.Append(dtName);
                sb.Append("\":[");
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        T obj = Activator.CreateInstance<T>();
                        PropertyInfo[] pi = obj.GetType().GetProperties();
                        sb.Append("{");
                        for (int j = 0; j < pi.Length; j++)
                        {
                            sb.Append("\"");
                            sb.Append(pi[j].Name.ToString());
                            sb.Append("\":\"");
                            if (pi[j].GetValue(list[i], null) != null && pi[j].GetValue(list[i], null) != DBNull.Value && pi[j].GetValue(list[i], null).ToString() != "")
                            {
                                sb.Append(pi[j].GetValue(list[i], null)).Replace("\\", "/");
                            }
                            else
                            {
                                sb.Append("");
                            }
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("},");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]}");
                return JsonCharFilter(sb.ToString());
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 表格返回JSON
        /// </summary>
        /// <param name="dt">数据行</param>
        /// <param name="pageIndex">当前页面</param>
        /// <param name="pqGrid_Sort">要显示字段</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string PqGridPageJson(DataTable dt, int pageIndex, string pqGrid_Sort, int count)
        {
            try
            {
                var sortField = pqGrid_Sort.Split(',');
                var sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"totalRecords\": " + count + ",");
                sb.Append("\"curPage\": " + pageIndex + ",");
                sb.Append("\"data\": [");
                if (DataTableHelper.IsExistRows(dt))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("[");
                        foreach (var item in sortField)
                        {
                            if (item.Length == 0)
                                continue;

                            sb.Append("\"");
                            if (dr[item] != null && dr[item] != DBNull.Value && dr[item].ToString() != "")
                                sb.Append(dr[item]);
                            else
                                sb.Append("");
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("],");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                sb.Append("}");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 表格返回JSON
        /// </summary>
        /// <param name="dt">数据行</param>
        /// <param name="pageIndex">当前页面</param>
        /// <param name="pqGrid_Sort">要显示字段</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string PqGridPageJson<T>(IList list, int pageIndex, string pqGrid_Sort, int count)
        {
            try
            {
                string[] Sort_Field = pqGrid_Sort.Split(',');
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"totalRecords\": " + count + ",");
                sb.Append("\"curPage\": " + pageIndex + ",");
                sb.Append("\"data\": [");
                if (list.Count > 0)
                {
                    foreach (T entity in list)
                    {
                        Hashtable ht = HashtableHelper.GetModelToHashtable<T>(entity);
                        sb.Append("[");
                        foreach (string item in Sort_Field)
                        {
                            sb.Append("\"");
                            if (ht[item] != null && ht[item] != DBNull.Value && ht[item].ToString() != "")
                            {
                                if (ht[item].ToString().Contains("\""))
                                    sb.Append(ht[item].ToString().Replace("\"","\\\""));
                                else
                                    sb.Append(ht[item]);
                            }
                            else
                                sb.Append("");
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("],");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                sb.Append("}");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 表格返回JSON
        /// </summary>
        /// <param name="pqGrid_Sort">要显示字段</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string PqGridJson(DataTable dt, string pqGrid_Sort)
        {
            try
            {
                string[] Sort_Field = pqGrid_Sort.Split(',');
                StringBuilder sb = new StringBuilder();
                if (DataTableHelper.IsExistRows(dt))
                {
                    sb.Append("[");
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("[");
                        foreach (string item in Sort_Field)
                        {
                            sb.Append("\"");
                            if (dr[item] != null && dr[item] != DBNull.Value && dr[item].ToString() != "")
                                sb.Append(dr[item]);
                            else
                                sb.Append("");
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("],");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append("]");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 表格返回JSON
        /// </summary>
        /// <param name="pqGrid_Sort">要显示字段</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string PqGridJson<T>(IList list, string pqGrid_Sort)
        {
            try
            {
                string[] Sort_Field = pqGrid_Sort.Split(',');
                StringBuilder sb = new StringBuilder();
                if (list.Count > 0)
                {
                    sb.Append("[");
                    foreach (T entity in list)
                    {
                        Hashtable ht = HashtableHelper.GetModelToHashtable<T>(entity);
                        sb.Append("[");
                        foreach (string item in Sort_Field)
                        {
                            sb.Append("\"");
                            if (ht[item] != null && ht[item] != DBNull.Value && ht[item].ToString() != "")
                                sb.Append(ht[item]);
                            else
                                sb.Append("");
                            sb.Append("\",");
                        }
                        sb = sb.Remove(sb.Length - 1, 1);
                        sb.Append("],");
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append("]");
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>
        /// 表格返回JSON
        /// </summary>
        /// <param name="pqGrid_Sort">要显示字段</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GridTable(DataTable dt, string pqGrid_Sort)
        {
            try
            {
                string[] Sort_Field = pqGrid_Sort.Split(',');
                StringBuilder sb = new StringBuilder();
                if (DataTableHelper.IsExistRows(dt))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (string item in Sort_Field)
                        {
                            if (dr[item] != null && dr[item] != DBNull.Value && dr[item].ToString() != "")
                                sb.Append("<td>" + dr[item] + "</td>");
                            else
                                sb.Append("<td></td>");
                        }
                        sb.Append("</tr>");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
                return "";
            }
        }
        /// <summary>  
        /// Json特符字符过滤
        /// </summary>  
        /// <param name="sourceStr">要过滤的源字符串</param>  
        /// <returns>返回过滤的字符串</returns>  
        private static string JsonCharFilter(string sourceStr)
        {
            return sourceStr;
        }
    }
}
