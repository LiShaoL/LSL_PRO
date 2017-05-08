 using LSL_PRO.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSL_PRO.DataAccess
{
    /// <summary>
    /// 有关数据库连接的接口
    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public interface IDbHelper
    {
        #region 根据 SQL 返回影响行数
        /// <summary>
        /// 根据SQL返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        object GetObjectValue(StringBuilder sql);
        /// <summary>
        /// 根据SQL返回影响行数,带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        object GetObjectValue(StringBuilder sql, SqlParam[] param);
        #endregion

        #region GetTableColumns

        /// <summary>
        /// 获取表结构 Add By mashanlin 2014-11-11
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名:可以为空</param>
        /// <returns></returns>
        DataTable GetTableColumns(string tableName,string columnName);
        #endregion

        #region 根据 SQL 执行
        /// <summary>
        ///  根据SQL执行
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>object</returns>
        int ExecuteBySql(StringBuilder sql);
        /// <summary>
        ///  根据SQL执行,带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>object</returns>
        int ExecuteBySql(StringBuilder sql, SqlParam[] param);
        /// <summary>
        ///  根据SQL执行,带参数,不带事务
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>object</returns>
        int ExecuteBySqlNotTran(StringBuilder sql, SqlParam[] param);
        /// <summary>
        /// 批量执行SQL语句
        /// </summary>
        /// <param name="sqls">sql语句</param>
        /// <param name="m_param">参数化</param>
        /// <returns></returns>
        int BatchExecuteBySql(object[] sqls, object[] param);
        #endregion

        #region 根据 SQL 返回 DataTable 数据集
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        DataTable GetDataTableBySQL(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataTable</returns>
        DataTable GetDataTableBySQL(StringBuilder sql, SqlParam[] param);
        #endregion

        #region 根据 储过程 返回 DataTable 数据集
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataTable
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        DataTable GetDataTableProc(string procName, Hashtable ht);
        /// <summary>
        /// 执行一存储过程返回数据表 返回多个值
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="rs">Hashtable</param>
        DataTable GetDataTableProcReturn(string procName, Hashtable ht, ref Hashtable rs);
        #endregion

        #region 根据 SQL 返回 DataSet 数据集
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSetBySQL(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSetBySQL(StringBuilder sql, SqlParam[] param);
        #endregion

        #region 根据 储过程 返回 DataSet 数据集
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataSet
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        DataSet GetDataSetProc(string procName, Hashtable ht);
        /// <summary>
        /// 执行一存储过程返回数据集 返回多个值
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="rs">Hashtable</param>
        DataSet GetDataSetProcReturn(string procName, Hashtable ht, ref Hashtable rs);
        #endregion

        #region 根据 SQL 返回 IList
        /// <summary>
        /// 根据 SQL 返回 IList
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        IList GetDataListBySQL<T>(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 IList,带参数 (比DataSet效率高)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        IList GetDataListBySQL<T>(StringBuilder sql, SqlParam[] param);
        #endregion

        #region 根据 存储过程 执行
        /// <summary>
        /// 调用存储过程(带事务)
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">参数化</param>
        int ExecuteByProc(string procName, Hashtable ht);
        /// <summary>
        ///调用存储过程 (不带事务)
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">参数化</param>
        /// <returns></returns>
        int ExecuteByProcNotTran(string procName, Hashtable ht);
        /// <summary>
        /// 批量调用存储过程
        /// </summary>
        /// <param name="text"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int BatchExecuteByProc(object[] text, object[] param);
        /// <summary>
        /// 调用存储过程返回指定消息
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut Msg</param>
        int ExecuteByProcReturnMsg(string procName, Hashtable ht, ref object msg);
        /// <summary>
        /// 调用存储过程返回指定消息（不带事务）
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut Msg</param>
        int ExecuteByProcNotTranReturnMsg(string procName, Hashtable ht, ref object msg);
        /// <summary>
        /// 调用存储过程返回指定消息
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut rs</param>
        int ExecuteByProcReturn(string procName, Hashtable ht, ref Hashtable rs);
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
        DataTable GetPageList(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
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
        DataTable GetPageList(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
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
        IList GetPageList<T>(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
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
        IList GetPageList<T>(string sql, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
        #endregion

        #region SqlBulkCopy 批量数据处理
        /// <summary>
        ///大批量数据插入
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        bool BulkInsert(DataTable dt, string table);
        #endregion
    }
}
