
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;

namespace LSL_PRO.Kernel
{
    /// <summary>
    /// 公共帮助类

    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class DbCommon
    {
        #region 类型转换
        /// <summary>
        /// 取得Int值,如果为Null 则返回０
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(object obj)
        {
            if (obj != null)
            {
                int i;
                int.TryParse(obj.ToString(), out i);
                return i;
            }
            else
                return 0;
        }

        public static float GetFloat(object obj)
        {
            float i;
            float.TryParse(obj.ToString(), out i);
            return i;
        }

        /// <summary>
        /// 取得Int值,如果不成功则返回指定exceptionvalue值
        /// </summary>
        /// <param name="obj">要计算的值</param>
        /// <param name="exceptionvalue">异常时的返回值</param>
        /// <returns></returns>
        public static int GetInt(object obj, int exceptionvalue)
        {
            if (obj == null)
                return exceptionvalue;
            if (string.IsNullOrEmpty(obj.ToString()))
                return exceptionvalue;
            int i = exceptionvalue;
            try { i = Convert.ToInt32(obj); }
            catch { i = exceptionvalue; }
            return i;
        }

        /// <summary>
        /// 取得byte值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte Getbyte(object obj)
        {
            if (obj.ToString() != "")
                return byte.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 获得Long值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(object obj)
        {
            if (obj.ToString() != "")
                return long.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 取得Long值,如果不成功则返回指定exceptionvalue值
        /// </summary>
        /// <param name="obj">要计算的值</param>
        /// <param name="exceptionvalue">异常时的返回值</param>
        /// <returns></returns>
        public static long GetLong(object obj, long exceptionvalue)
        {
            if (obj == null)
            {
                return exceptionvalue;
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return exceptionvalue;
            }
            long i = exceptionvalue;
            try
            {
                i = Convert.ToInt64(obj);
            }
            catch
            {
                i = exceptionvalue;
            }
            return i;
        }

        /// <summary>
        /// 取得Decimal值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            if (obj.ToString() != "")
                return decimal.Parse(obj.ToString());
            else
                return 0;
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            if (obj.ToString() != "")
                return DateTime.Parse(obj.ToString());
            else
                return DateTime.Now;
            //return DateTime.MinValue;
        }
        /// <summary>
        /// 格式化日期 yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetFormatDateTime(object obj, string Format)
        {
            if (obj.ToString() != "")
                return DateTime.Parse(obj.ToString()).ToString(Format);
            else
                return "";
        }
        /// <summary>
        /// 取得bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBool(object obj)
        {
            if (obj != null)
            {
                bool flag;
                bool.TryParse(obj.ToString(), out flag);
                return flag;
            }
            else
                return false;
        }

        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] GetByte(object obj)
        {
            if (obj.ToString() != "")
            {
                return (Byte[])obj;
            }
            else
                return null;
        }

        /// <summary>
        /// 取得string值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            if (obj != null && obj != DBNull.Value)
                return obj.ToString();
            else
                return "";
        }
        #endregion

        #region DataTable 转 Hashtable
        /// <summary>
        /// DataTable 转 DataTableToHashtable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Hashtable DataTableToHashtable(DataTable dt)
        {
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string key = dt.Columns[i].ColumnName;
                    ht[key.ToUpper()] = dr[key];
                }
            }
            return ht;
        }
        #endregion

        #region 对象参数转换SqlParam
        /// <summary>
        /// Hashtable对象参数转换
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static SqlParam[] GetParameter(Hashtable ht)
        {
             
            DbType dbtype = new DbType();
            IList<SqlParam> sqlparam = new List<SqlParam>();
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    if (ht[key] is DateTime)
                        dbtype = DbType.DateTime;
                    else
                        dbtype = DbType.String;
                    sqlparam.Add(new SqlParam(ParamKey + key, dbtype, ht[key]));
                }
            }
            return sqlparam.ToArray();
        }
        /// <summary>
        /// 实体类对象参数转换
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static SqlParam[] GetParameter<T>(T entity)
        {
             
            DbType dbtype = new DbType();
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            IList<SqlParam> sqlparam = new List<SqlParam>();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.PropertyType.ToString() == "System.Nullable`1[System.DateTime]")
                        dbtype = DbType.DateTime;
                    else
                        dbtype = DbType.String;
                    sqlparam.Add(new SqlParam(ParamKey + prop.Name, dbtype, prop.GetValue(entity, null)));
                }
            }
            return sqlparam.ToArray();
        }
        #endregion

        #region 通过参数类构造键值
        /// <summary>
        /// 通过参数类构造键值,MYSQL
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        public static void MySqlAddInParameter(DbCommand dbCommand, Hashtable ht)
        {
            MySqlParameter parameters = new MySqlParameter();
            if (ht == null) return;
            foreach (string key in ht.Keys)
            {
                dbCommand.Parameters.Add(new MySqlParameter(ParamKey + key, ht[key]));
            }
        }
        /// <summary>
        /// 通过参数类构造键值,MYSQL
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        public static void MySqlAddInParameter(DbCommand dbCommand, SqlParam[] param)
        {
            MySqlParameter parameters = new MySqlParameter();
            if (param == null) return;
            foreach (SqlParam _param in param)
            {
                dbCommand.Parameters.Add(new MySqlParameter(_param.FieldName, _param.FiledValue));
            }
        }
        /// <summary>
        /// 通过参数类构造键值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        public static void AddInParameter(Database database, DbCommand cmd, SqlParam[] _params)
        {
             
            if (_params == null) return;
            DbType dbtype = new DbType();
            foreach (SqlParam _param in _params)
            {
                if (_param.FiledValue is DateTime)
                    dbtype = DbType.DateTime;
                else
                    dbtype = DbType.String;
                database.AddInParameter(cmd, _param.FieldName, dbtype, _param.FiledValue);
            }
        }
        /// <summary>
        /// 通过Hashtable对象构造键值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        public static void AddInParameter(Database database, DbCommand cmd, Hashtable ht)
        {
             
            if (ht == null) return;
            foreach (string key in ht.Keys)
            {
                if (key == "Msg")
                {
                    database.AddOutParameter(cmd, ParamKey + key, DbType.String, 1000);
                }
                else
                {
                    database.AddInParameter(cmd, ParamKey + key, DbType.String, ht[key]);
                }
            }
        }

        /// <summary>
        /// 通过Hashtable对象构造键值
        /// </summary>
        /// <param name="database">数据库</param>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        public static void AddMoreParameter(Database database, DbCommand cmd, Hashtable ht)
        {
             
            if (ht == null) return;
            foreach (string key in ht.Keys)
            {
                if (key.StartsWith("OUT_"))
                {
                    string tmp = key.Remove(0, 4);
                    database.AddOutParameter(cmd, ParamKey + tmp, DbType.String, 1000);
                }
                else
                {
                    database.AddInParameter(cmd, ParamKey + key, DbType.String, ht[key]);
                }
            }
        }
        #endregion

        #region 有关数据库关键字
        /// <summary>
        ///  获得Sql字符串相加符号
        ///             Oracle ||
        ///             SQLServer +
        ///             MySql +
        /// </summary>
        /// <returns>字符加</returns>
        public static string PlusSign { get; set; }
        /// <summary>
        /// 获得数据库日期时间
        ///             Oracle：SYSDATE()
        ///             SQLServer：GETDATE()
        ///             MySql：NOW()
        /// </summary>
        /// <returns>日期时间</returns>
        public static string GetDBNow { get; set; }
        /// <summary>
        /// 获得数据库参数化
        ///             Oracle :
        ///             SQLServer @
        ///             MySql ?
        /// </summary>
        public static string ParamKey { get; set; }
        #endregion

        #region 拼接 新增 SQL语句
        /// <summary>
        /// 哈希表生成InsertSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="ParamKey">
        ///             参数化
        ///             Oracle :
        ///             SQLServer @
        ///             MySql ?
        /// </param>
        /// <returns>int</returns>
        public static StringBuilder InsertSql(string tableName, Hashtable ht)
        {
             
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    sb_prame.Append("," + key);
                    sp.Append("," + ParamKey + "" + key);
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }
        /// <summary>
        /// 泛型方法，反射生成InsertSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ParamKey">
        ///             参数化
        ///             Oracle :
        ///             SQLServer @
        ///             MySql ?
        /// </param>
        /// <returns>int</returns>
        public static StringBuilder InsertSql<T>(T entity)
        {
             
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(type.Name);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    sb_prame.Append("," + (prop.Name));
                    sp.Append("," + ParamKey + "" + (prop.Name));
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }
        #endregion

        #region 拼接 修改 SQL语句
        /// <summary>
        /// 哈希表生成UpdateSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">主键</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="ParamKey">
        ///             参数化
        ///             Oracle :
        ///             SQLServer @
        ///             MySql ?
        /// </param>
        /// <returns></returns>
        public static StringBuilder UpdateSql(string tableName, string pkName, Hashtable ht)
        {
             
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(key);
                        sb.Append("=");
                        sb.Append(ParamKey + key);
                    }
                    else
                    {
                        sb.Append("," + key);
                        sb.Append("=");
                        sb.Append(ParamKey + key);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(ParamKey + pkName);
            return sb;
        }
        /// <summary>
        /// 泛型方法，反射生成UpdateSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="pkName">主键</param>
        /// <param name="ParamKey">
        ///             参数化
        ///             Oracle :
        ///             SQLServer @
        ///             MySql ?
        /// </param>
        /// <returns>int</returns>
        public static StringBuilder UpdateSql<T>(T entity, string pkName)
        {
             
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(prop.Name);
                        sb.Append("=");
                        sb.Append(ParamKey + prop.Name);
                    }
                    else
                    {
                        sb.Append("," + prop.Name);
                        sb.Append("=");
                        sb.Append(ParamKey + prop.Name);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(ParamKey + pkName);
            return sb;
        }
        #endregion

        #region 拼接 删除 SQL语句
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql(string tableName, string pkName)
        {
             
            StringBuilder sb = new StringBuilder("Delete From " + tableName + " Where " + pkName + " = " + ParamKey + pkName + "");
            return sb;
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">多参数</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql(string tableName, Hashtable ht)
        {
             
            StringBuilder sb = new StringBuilder("Delete From " + tableName + " Where 1=1");
            foreach (string key in ht.Keys)
            {
                sb.Append(" AND " + key + " = " + DbCommon.ParamKey + "" + key + "");
            }
            return sb;
        }
        #endregion
    }
}

