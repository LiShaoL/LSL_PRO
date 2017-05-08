using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using LSL_PRO.Kernel;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using MySql.Data.MySqlClient;
using System.Linq;

namespace LSL_PRO.DataAccess
{
    /// <summary>
    /// 有关数据库连接的方法
    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class SqlServerHelper : IDbHelper, IDisposable
    {
        #region 数据库连接必要条件参数
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connectionString = "";
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connString">数据库连接字符串</param> Oracle,,
        public SqlServerHelper(string connString, string sqlSourceType)
        {
            if (sqlSourceType == "SQLServer") 
            {
                DbCommon.ParamKey = "@";
                DbCommon.PlusSign = "+";
                DbCommon.GetDBNow = "GETDATE()";
                connectionString = connString;
                new SqlSourceType();
            }
            else if (sqlSourceType == "Oracle")
            {
                DbCommon.ParamKey = ":";
                DbCommon.PlusSign = "||";
                DbCommon.GetDBNow = "SYSDATE()";
                connectionString = connString;
                new SqlSourceType();
            }
            else if (sqlSourceType == "MySql")
            {
                DbCommon.ParamKey = "?";
                DbCommon.PlusSign = "+";
                DbCommon.GetDBNow = "NOW()";
                connectionString = connString;
                new SqlSourceType();
            }
            
        }
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        public SqlSourceType SqlSourceType
        {
            get
            {
                return SqlSourceType.SQLServer;
            }
        }
        /// <summary>
        /// 对象用于锁
        /// </summary>
        private static readonly Object locker = new Object();
        private SqlDatabase db = null;
        /// <summary>
        /// 取得单身实例
        /// </summary>
        public SqlDatabase GetInstance()
        {
            //在并发时，使用单一对象
            if (db == null)
            {
                return db = new SqlDatabase(connectionString);
            }
            else
            {
                lock (locker)
                {
                    return db;
                }
            }
        }
        //DbCommand 命令
        private DbCommand dbCommand = null;
        /// <summary>
        /// 命令
        /// </summary>
        public DbCommand DbCommand
        {
            get
            {
                return this.dbCommand;
            }
            set
            {
                this.dbCommand = value;
            }
        }
        //事务 命令
        private DbTransaction dbTransaction = null;
        // 是否已在事务之中
        private bool inTransaction = false;
        /// <summary>
        /// 是否已采用事务
        /// </summary>
        public bool InTransaction
        {
            get
            {
                return this.inTransaction;
            }
            set
            {
                this.inTransaction = value;
            }
        }
        #endregion

        #region 根据 SQL 返回影响行数
        /// <summary>
        /// 根据SQL返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public object GetObjectValue(StringBuilder sql)
        {
            return this.GetObjectValue(sql, null);
        }
        /// <summary>
        /// 根据SQL返回影响行数,带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        public object GetObjectValue(StringBuilder sql, SqlParam[] param)
        {
            try
            {
                dbCommand = this.GetInstance().GetSqlStringCommand(sql.ToString());
                DbCommon.AddInParameter(db, dbCommand, param);
                return db.ExecuteScalar(dbCommand);
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        #endregion

        #region 根据 SQL 执行
        /// <summary>
        ///  根据SQL执行
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>object</returns>
        public int ExecuteBySql(StringBuilder sql)
        {
            return this.ExecuteBySql(sql, null);
        }
        /// <summary>
        ///  根据SQL执行,带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>object</returns>
        public int ExecuteBySql(StringBuilder sql, SqlParam[] param)
        {
            int num = 0;
            try
            {
                dbCommand = this.GetInstance().GetSqlStringCommand(sql.ToString());
                DbCommon.AddInParameter(db, dbCommand, param);
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    dbTransaction = connection.BeginTransaction();
                    try
                    {
                        num = db.ExecuteNonQuery(dbCommand, dbTransaction);
                        dbTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        num = -1;
                        DbLog.WriteException(e);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        ///  根据SQL执行,带参数,不带事务
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>object</returns>
        public int ExecuteBySqlNotTran(StringBuilder sql, SqlParam[] param)
        {
            int num = 0;
            try
            {
                dbCommand = this.GetInstance().GetSqlStringCommand(sql.ToString());
                DbCommon.AddInParameter(db, dbCommand, param);
                num = db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception e)
            {
                num = -1;
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="sqls">sql语句</param>
        /// <param name="m_param">参数化</param>
        /// <returns></returns>
        public int BatchExecuteBySql(object[] sqls, object[] param)
        {
            int num = 0;
            try
            {
                using (DbConnection connection = this.GetInstance().CreateConnection())
                {
                    connection.Open();
                    dbTransaction = connection.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < sqls.Length; i++)
                        {
                            StringBuilder builder = (StringBuilder)sqls[i];
                            if (builder != null)
                            {
                                SqlParam[] paramArray = (SqlParam[])param[i];
                                DbCommand sqlStringCommand = this.db.GetSqlStringCommand(builder.ToString());
                                DbCommon.AddInParameter(db, sqlStringCommand, paramArray);
                                this.db.ExecuteNonQuery(sqlStringCommand, dbTransaction);
                            }
                        }
                        dbTransaction.Commit();
                        num = 1;
                    }
                    catch (Exception e)
                    {
                        num = -1;
                        dbTransaction.Rollback();
                        DbLog.WriteException(e);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        #endregion

        #region GetTableColumns

        /// <summary>
        /// 获取表结构 Add By mashanlin 2014-11-11
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名:可以为空</param>
        /// <returns></returns>
        public DataTable GetTableColumns(string tableName, string columnName)
        {
            StringBuilder sql = new StringBuilder(
                String.Format(@"SELECT s1.name AS ColumnName,CASE s2.name WHEN 'nvarchar' THEN s1.[prec] ELSE s1.[length] END AS [MaxSize],isnullable AS [IsNullable],colstat AS [ReadOnly],s2.name AS [SqlType] ,isnull(s3.[value],'') AS DESCription 
                                FROM syscolumns s1 RIGHT JOIN systypes s2 ON s2.xtype =s1.xtype LEFT JOIN sys.extENDed_properties s3 ON ( s3.minor_id = s1.colid and s3.major_id = s1.id)
                                WHERE id=object_id('{0}') and s2.name<>'sysname' {1} 
                                ORDER BY ColumnName ",
                                tableName,
                                String.IsNullOrEmpty(columnName) ? "" : " and s1.name='" + columnName + "' "));

            return GetDataTableBySQL(sql);
        }
        #endregion

        #region 根据 SQL 返回 DataTable 数据集
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableBySQL(StringBuilder sql)
        {
            return this.GetDataTableBySQL(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableBySQL(StringBuilder sql, SqlParam[] param)
        {
            try
            {
                dbCommand = this.GetInstance().GetSqlStringCommand(sql.ToString());
                DbCommon.AddInParameter(db, dbCommand, param);
                return DbReader.ReaderToDataTable(db.ExecuteReader(dbCommand));
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        #endregion

        #region 根据 储过程 返回 DataTable 数据集
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataTable
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        public DataTable GetDataTableProc(string procName, Hashtable ht)
        {
            try
            {
                dbCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddInParameter(db, dbCommand, ht);
                return DbReader.ReaderToDataTable(db.ExecuteReader(dbCommand));
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        /// <summary>
        /// 执行一存储过程返回数据表 返回多个值
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="rs">Hashtable</param>
        public DataTable GetDataTableProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            try
            {
                dbCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddMoreParameter(db, dbCommand, ht);
                DataTable dt = DbReader.ReaderToDataTable(db.ExecuteReader(dbCommand));
                rs = new Hashtable();
                foreach (string key in ht.Keys)
                {
                    if (key.StartsWith("OUT_"))
                    {
                        string tmp = key.Remove(0, 4);
                        object val = db.GetParameterValue(dbCommand, "@" + tmp);
                        rs[key] = val;
                    }
                }
                return dt;
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        #endregion

        #region 根据 SQL 返回 DataSet 数据集
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetBySQL(StringBuilder sql)
        {
            return GetDataSetBySQL(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetBySQL(StringBuilder sql, SqlParam[] param)
        {
            try
            {
                dbCommand = this.GetInstance().GetSqlStringCommand(sql.ToString());
                DbCommon.AddInParameter(db, dbCommand, param);
                return db.ExecuteDataSet(dbCommand);
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        #endregion

        #region 根据 储过程 返回 DataSet 数据集
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataSet
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        public DataSet GetDataSetProc(string procName, Hashtable ht)
        {
            try
            {
                dbCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddInParameter(db, dbCommand, ht);
                DataSet ds = db.ExecuteDataSet(dbCommand);
                return ds;
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        /// <summary>
        /// 执行一存储过程返回数据集 返回多个值
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="rs">Hashtable</param>
        public DataSet GetDataSetProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            try
            {
                dbCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddMoreParameter(db, dbCommand, ht);
                DataSet ds = db.ExecuteDataSet(dbCommand);
                rs = new Hashtable();
                foreach (string key in ht.Keys)
                {
                    if (key.StartsWith("OUT_"))
                    {
                        string tmp = key.Remove(0, 4);
                        object val = db.GetParameterValue(dbCommand, "@" + tmp);
                        rs[key] = val;
                    }
                }
                return ds;
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null;
            }
        }
        #endregion

        #region 根据 SQL 返回 IList
        /// <summary>
        /// 根据 SQL 返回 IList
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public IList GetDataListBySQL<T>(StringBuilder sql)
        {
            return this.GetDataListBySQL<T>(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 IList,带参数 (比DataSet效率高)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        public IList GetDataListBySQL<T>(StringBuilder sql, SqlParam[] param)
        {
            dbCommand = this.GetInstance().GetSqlStringCommand(sql.ToString());
            DbCommon.AddInParameter(db, dbCommand, param);
            return DbReader.ReaderToList<T>(db.ExecuteReader(dbCommand));
        }
        #endregion

        #region 根据 存储过程 执行
        /// <summary>
        /// 调用存储过程(带事务)
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">参数化</param>
        public int ExecuteByProc(string procName, Hashtable ht)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddInParameter(db, storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    dbTransaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, dbTransaction);
                        dbTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        num = -1;
                        dbTransaction.Rollback();
                        DbLog.WriteException(e);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        ///调用存储过程 (不带事务)
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">参数化</param>
        /// <returns></returns>
        public int ExecuteByProcNotTran(string procName, Hashtable ht)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddInParameter(db, storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand);
                    }
                    catch (Exception e)
                    {
                        DbLog.WriteException(e);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        /// 批量调用存储过程
        /// </summary>
        /// <param name="text"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int BatchExecuteByProc(object[] text, object[] param)
        {
            int num = 0;
            try
            {
                using (DbConnection connection = this.GetInstance().CreateConnection())
                {
                    connection.Open();
                    dbTransaction = connection.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < text.Length; i++)
                        {
                            string strtext = text[i].ToString().ToUpper();
                            if (strtext != null)
                            {
                                SqlParam[] paramArray = (SqlParam[])param[i];
                                DbCommand command = null;
                                if (strtext.StartsWith("PROC_"))
                                {
                                    command = this.db.GetStoredProcCommand(strtext);
                                }
                                else
                                {
                                    command = this.db.GetSqlStringCommand(strtext);
                                }
                                DbCommon.AddInParameter(db, command, paramArray);
                                this.db.ExecuteNonQuery(command, dbTransaction);
                            }
                        }
                        dbTransaction.Commit();
                        num = 1;
                    }
                    catch (Exception e)
                    {
                        num = -1;
                        dbTransaction.Rollback();
                        DbLog.WriteException(e);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        /// 调用存储过程返回指定消息
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut Msg</param>
        public int ExecuteByProcReturnMsg(string procName, Hashtable ht, ref object msg)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddInParameter(db, storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    dbTransaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, dbTransaction);
                        dbTransaction.Commit();
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        num = -1;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                    }
                }
                msg = this.db.GetParameterValue(storedProcCommand, "@Msg");
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        /// 调用存储过程返回指定消息（不带事务）
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut Msg</param>
        public int ExecuteByProcNotTranReturnMsg(string procName, Hashtable ht, ref object msg)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddInParameter(db, storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    try
                    {
                        connection.Open();
                        num = this.db.ExecuteNonQuery(storedProcCommand);
                        num = 1;
                    }
                    catch (Exception e)
                    {
                        DbLog.WriteException(e);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
                msg = this.db.GetParameterValue(storedProcCommand, "@Msg");
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        /// <summary>
        /// 调用存储过程返回指定消息
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut rs</param>
        public int ExecuteByProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetInstance().GetStoredProcCommand(procName);
                DbCommon.AddMoreParameter(db, storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    dbTransaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, dbTransaction);
                        dbTransaction.Commit();
                    }
                    catch
                    {
                        num = -1;
                        dbTransaction.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                        dbTransaction.Dispose();
                    }
                }
                rs = new Hashtable();
                foreach (string str in ht.Keys)
                {
                    if (str.StartsWith("OUT_"))
                    {
                        object parameterValue = this.db.GetParameterValue(storedProcCommand, "@" + str.Remove(0, 4));
                        rs[str] = parameterValue;
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
            }
            return num;
        }
        #endregion

        #region 数据分页 返回 DataTable
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     param：参数化
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        public DataTable GetPageList(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sb.Append("Select * From (Select ROW_NUMBER() Over (Order By " + orderField + " " + orderType + "");
                sb.Append(") As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
                count = Convert.ToInt32(this.GetObjectValue(new StringBuilder("Select Count(1) From (" + sql + ") As t"), param));
                return this.GetDataTableBySQL(sb, param);
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null; ;
            }
        }
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        public DataTable GetPageList(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            return GetPageList(sql, null, orderField, orderType, pageIndex, pageSize, ref  count);
        }
        #endregion

        #region 数据分页 返回 ILis
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     param：参数化
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        public IList GetPageList<T>(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sb.Append("Select * From (Select ROW_NUMBER() Over (Order By " + orderField + " " + orderType + "");
                sb.Append(") As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
                count = Convert.ToInt32(this.GetObjectValue(new StringBuilder("Select Count(1) From (" + sql + ") As t"), param));
                return this.GetDataListBySQL<T>(sb, param);
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return null; ;
            }
        }
        /// <summary>
        /// 摘要:
        ///     数据分页
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        public IList GetPageList<T>(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            return GetPageList<T>(sql, null, orderField, orderType, pageIndex, pageSize, ref  count);
        }
        #endregion

        #region SqlBulkCopy 批量数据处理
        /// <summary>
        ///SqlServer大批量数据插入(其他数据库未写)
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        public bool BulkInsert(DataTable dt, string table)
        {
            return SqlServerBulkInsert(dt, table, connectionString);
        }
        /// <summary>
        /// 批量操作每批次记录数
        /// </summary>
        private static int BatchSize = 2000;

        /// <summary>
        /// 超时时间
        /// </summary>
        private int CommandTimeOut = 600;
        #region Oracle
        /// <summary>
        ///大批量数据插入
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        private bool OracleBulkInsert(DataTable table, string connectionString)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    OracleTransaction trans = connection.BeginTransaction();
                    using (OracleBulkCopy bulkCopy = new OracleBulkCopy(connection))
                    {
                        //设置源表名称
                        bulkCopy.DestinationTableName = table.TableName;
                        //设置超时限制
                        bulkCopy.BulkCopyTimeout = CommandTimeOut;
                        //要写入列
                        foreach (DataColumn dtColumn in table.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(dtColumn.ColumnName.ToUpper(), dtColumn.ColumnName.ToUpper());
                        }
                        try
                        {
                            // 写入
                            bulkCopy.WriteToServer(table);
                            // 提交事务
                            trans.Commit();
                            return true;
                        }
                        catch
                        {
                            trans.Rollback();
                            bulkCopy.Close();
                            return false;
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                            bulkCopy.Close();
                            bulkCopy.Dispose();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return false;
            }
        }
        #endregion

        #region SqlServer

        /// <summary>
        ///SqlServer大批量数据插入
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="tbname">数据表名</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        private bool SqlServerBulkInsert(DataTable table, string tbname, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction trans = conn.BeginTransaction();

                    SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
                    // 设置源表名称
                    sqlbulkCopy.DestinationTableName = tbname;
                    //分几次拷贝
                    //sqlbulkCopy.BatchSize = 10;
                    // 设置超时限制
                    sqlbulkCopy.BulkCopyTimeout = CommandTimeOut;
                    foreach (DataColumn dtColumn in table.Columns)
                    {
                        sqlbulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                    }
                    try
                    {
                        // 写入
                        sqlbulkCopy.WriteToServer(table);
                        // 提交事务
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        sqlbulkCopy.Close();
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                        sqlbulkCopy.Close();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return false;
            }
        }
        #endregion

        #region MySql
        /// <summary>
        ///大批量数据插入
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        private bool MySqlBulkInsert(DataTable table, string connectionString)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlTransaction tran = null;
                    try
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                        {
                            FieldTerminator = ",",
                            FieldQuotationCharacter = '"',
                            EscapeCharacter = '"',
                            LineTerminator = "\r\n",
                            NumberOfLinesToSkip = 0,
                            TableName = table.TableName,
                        };
                        bulk.Timeout = CommandTimeOut;
                        bulk.Columns.AddRange(table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                DbLog.WriteException(e);
                return false;
            }
        }
        /// <summary>
        ///使用MySqlDataAdapter批量更新数据
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        private void BatchUpdate(string connectionString, DataTable table)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandTimeout = CommandTimeOut;
            command.CommandType = CommandType.Text;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            MySqlCommandBuilder commandBulider = new MySqlCommandBuilder(adapter);
            commandBulider.ConflictOption = ConflictOption.OverwriteChanges;
            MySqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                //设置批量更新的每次处理条数
                adapter.UpdateBatchSize = BatchSize;
                //设置事物
                adapter.SelectCommand.Transaction = transaction;
                if (table.ExtendedProperties["SQL"] != null)
                {
                    adapter.SelectCommand.CommandText = table.ExtendedProperties["SQL"].ToString();
                }
                adapter.Update(table);
                transaction.Commit();/////提交事务
            }
            catch (MySqlException ex)
            {
                if (transaction != null) transaction.Rollback();
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
        #endregion
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 内存回收
        /// </summary>
        public void Dispose()
        {
            if (this.dbCommand != null)
            {
                this.dbCommand.Dispose();
            }
            if (this.dbTransaction != null)
            {
                this.dbTransaction.Dispose();
            }
        }
        #endregion
    }
}
