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
    /// 增、删、改、查
    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class DbUtilities : IDbUtilities
    {
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        public SqlSourceType SqlSourceType { get; set; }
        private IDbHelper db = null;
        /// <summary>
        /// 链接数据库实例
        /// </summary>
        /// <returns></returns>
        public IDbHelper GetInstance()
        {
            if (db == null)
            {
                return db = new SqlServerHelper(connectionString, SqlSourceType.ToString());
            }
            else
            {
                return db;
            }
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connectionString = "";
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="type">数据库软件类型</param>
        public DbUtilities(string connString, SqlSourceType type)
        {
            connectionString = connString;
            SqlSourceType = type;
        }

        #region 根据唯一ID获取对象,返回实体
        /// <summary>
        /// 根据唯一ID获取对象,返回Hashtable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns>返回Hashtable</returns>
        public Hashtable GetHashtableById(string tableName, string pkName, string pkVal)
        {
            this.GetInstance();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ").Append(tableName).Append(" Where ").Append(pkName).Append("=" + DbCommon.ParamKey + "ID");
            return DbCommon.DataTableToHashtable(db.GetDataTableBySQL(sb, new SqlParam[] { new SqlParam("" + DbCommon.ParamKey + "ID", pkVal) }));
        }
        /// <summary>
        /// 根据唯一ID获取对象,返回Hashtable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">参数</param>
        /// <returns>返回Hashtable</returns>
        public Hashtable GetHashtableById(string tableName, Hashtable ht)
        {
            this.GetInstance();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + tableName + " WHERE 1=1");
            foreach (string key in ht.Keys)
            {
                strSql.Append(" AND " + key + " = " + DbCommon.ParamKey + "" + key + "");
            }
            return DbCommon.DataTableToHashtable(db.GetDataTableBySQL(strSql, DbCommon.GetParameter(ht)));
        }
        /// <summary>
        /// 根据唯一ID获取对象,返回Hashtable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">条件</param>
        /// <param name="param">参数化</param>
        /// <returns>返回Hashtable</returns>
        public Hashtable GetHashtableById(string tableName, StringBuilder where, SqlParam[] param)
        {
            this.GetInstance();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + tableName + " WHERE 1=1");
            strSql.Append(where);
            return DbCommon.DataTableToHashtable(db.GetDataTableBySQL(strSql, param));
        }
        /// <summary>
        /// 根据字段获取对象,返回实体
        /// </summary>
        /// <param name="pkName">字段名</param>
        /// <param name="pkVal">字段值</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(string pkName, string pkVal)
        {
            if (string.IsNullOrEmpty(pkVal))
            {
                return default(T);
            }
            this.GetInstance();
            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            // if(type.getc)
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM ").Append(type.Name).Append(" Where ").Append(pkName).Append("=" + DbCommon.ParamKey + pkName);
            DataTable dt = db.GetDataTableBySQL(sb, new SqlParam[] { new SqlParam("" + DbCommon.ParamKey + pkName, pkVal) });
            if(dt!=null)
            {
                if (dt.Rows.Count > 0)
                {
                    return DbReader.ReaderToModel<T>(dt.Rows[0]);
                }
            }
            return model;
        }
        /// <summary>
        /// 根据唯一ID获取对象,返回实体
        /// </summary>
        /// <param name="ht">参数</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(Hashtable ht)
        {
            this.GetInstance();
            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + type.Name + " WHERE 1=1");
            foreach (string key in ht.Keys)
            {
                strSql.Append(" AND " + key + " = " + DbCommon.ParamKey + "" + key + "");
            }
            DataTable dt = db.GetDataTableBySQL(strSql, DbCommon.GetParameter(ht));
            if (dt.Rows.Count > 0)
            {
                return DbReader.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }
        /// <summary>
        /// 根据唯一ID获取对象,返回实体
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="param">参数化</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(StringBuilder where, SqlParam[] param)
        {
            this.GetInstance();
            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + type.Name + " WHERE 1=1");
            strSql.Append(where);
            DataTable dt = db.GetDataTableBySQL(strSql, param);
            if (dt.Rows.Count > 0)
            {
                return DbReader.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }

        /// <summary>
        /// 根据唯一ID获取对象,返回实体
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="param">参数化</param>
        /// /// <param name="viewName">视图名称</param>
        /// <returns>返回实体类</returns>
        public T GetModelById<T>(StringBuilder where, SqlParam[] param, string viewName)
        {
            this.GetInstance();
            T model = Activator.CreateInstance<T>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + viewName + " WHERE 1=1");
            strSql.Append(where);
            DataTable dt = db.GetDataTableBySQL(strSql, param);
            if (dt.Rows.Count > 0)
            {
                return DbReader.ReaderToModel<T>(dt.Rows[0]);
            }
            return model;
        }
        #endregion

        #region RecordCount
        /// <summary>
        /// 影响行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns>返回数量</returns>
        public int RecordCount(string tableName, string pkName, string pkVal)
        {
            this.GetInstance();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(1) FROM " + tableName + "");
            strSql.Append(" where " + pkName + " = " + DbCommon.ParamKey + pkName);
            SqlParam[] param = {
                                         new SqlParam(DbCommon.ParamKey+pkName+"",pkVal)};
            return DbCommon.GetInt(db.GetObjectValue(strSql, param));
        }
        /// <summary>
        /// 影响行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">参数</param>
        /// <returns>返回数量</returns>
        public int RecordCount(string tableName, Hashtable ht)
        {
            this.GetInstance();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(1) FROM " + tableName + " WHERE 1=1");
            foreach (string key in ht.Keys)
            {
                strSql.Append(" AND " + key + " = " + DbCommon.ParamKey + "" + key + "");
            }
            return DbCommon.GetInt(db.GetObjectValue(strSql, DbCommon.GetParameter(ht)));
        }
        /// <summary>
        /// 影响行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">条件</param>
        /// <param name="param">参数化</param>
        /// <returns>返回数量</returns>
        public int RecordCount(string tableName, StringBuilder where, SqlParam[] param)
        {
            this.GetInstance();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(1) FROM " + tableName + " WHERE 1=1 ");
            strSql.Append(where);
            return DbCommon.GetInt(db.GetObjectValue(strSql, param));
        }
        /// <summary>
        /// 记录条数 Add My mashanlin 2015-04-30
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int RecordCount(StringBuilder sql)
        {
            this.GetInstance();
            return DbCommon.GetInt(db.GetObjectValue(sql));
        }

        #endregion

        #region GetMax
        /// <summary>
        /// 获取最大编号
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段</param>
        /// <returns></returns>
        public object GetMax(string tableName, string pkName)
        {
            this.GetInstance();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT MAX(" + pkName + ") FROM " + tableName + "");
            object obj = db.GetObjectValue(strSql);
            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                return Convert.ToInt32(obj) + 1;
            }
            return 1;
        }
        #endregion

        #region Insert
        /// <summary>
        /// 通过Hashtable插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public int Insert(string tableName, Hashtable ht)
        {
            this.GetInstance();
            return db.ExecuteBySql(DbCommon.InsertSql(tableName, ht), DbCommon.GetParameter(ht));
        }
        /// <summary>
        /// 通过实体类插入数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>int</returns>
        public int Insert<T>(T entity)
        {
            this.GetInstance();
            int result = db.ExecuteBySql(DbCommon.InsertSql(entity), DbCommon.GetParameter(entity));
            return result;
        }
        /// <summary>
        /// 表单提交：新增
        ///     参数：
        ///     tableName:表名
        ///     pkName：字段主键
        ///     pkValue：字段值
        ///     ht：参数
        /// </summary>
        /// <returns></returns>
        public bool AddByHashTable(string tableName, string pkName, string pkValue, Hashtable ht)
        {
            this.GetInstance();
            if (this.InsertByHashtable(tableName, ht) > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 通过Hashtable插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public virtual int InsertByHashtable(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb_prame.Append("," + key);
                sp.Append("," + DbCommon.ParamKey + "" + key);
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            int _object = db.ExecuteBySql(sb, DbCommon.GetParameter(ht));
            return _object;
        }
        #endregion

        #region Update
        /// <summary>
        /// 通过Hashtable修改数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkValue"></param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public int Update(string tableName, string pkName, string pkVal, Hashtable ht)
        {
            ht[pkName] = pkVal;
            this.GetInstance();
            return db.ExecuteBySql(DbCommon.UpdateSql(tableName, pkName, ht), DbCommon.GetParameter(ht));
        }

        /// <summary>
        /// 通过实体类修改数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int Update<T>(T entity, string key)
        {
            this.GetInstance();
            return db.ExecuteBySql(DbCommon.UpdateSql(entity, key), DbCommon.GetParameter(entity));
        }
       
        /// <summary>
        /// 表单提交：修改 
        ///     参数：
        ///     tableName:表名
        ///     pkName：字段主键
        ///     pkValue：字段值
        ///     ht：参数
        /// </summary>
        /// <returns></returns>
        public bool UpdateByHashTable(string tableName, string pkName, string pkValue, Hashtable ht)
        {
            this.GetInstance();
            if (this.Update(tableName, pkName, pkValue, ht) > 0)
                return true;
            else
                return false;
        }
        
        #endregion

        #region Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int Delete(string tableName, string pkName, string pkVal)
        {
            this.GetInstance();
            return db.ExecuteBySql(DbCommon.DeleteSql(tableName, pkName), new SqlParam[] { new SqlParam(DbCommon.ParamKey + pkName, pkVal) });
        }
        /// <summary>
        /// 批量删除 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="whereStr">条件</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public int BatchDelete(string tableName, string whereStr, SqlParam[] param)
        {
            this.GetInstance();
            int index = 0;
            if (tableName.Contains("1=1"))
            {
                return 0;
            }
            string str = DbCommon.ParamKey + "ID" + index;
            StringBuilder sql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + whereStr);
            return db.ExecuteBySql(sql, param);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int BatchDelete(string tableName, string pkName, object[] pkValues)
        {
            this.GetInstance();
            SqlParam[] param = new SqlParam[pkValues.Length];
            int index = 0;
            string str = DbCommon.ParamKey + "ID" + index;
            StringBuilder sql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + pkName + " IN (");
            for (int i = 0; i < (param.Length - 1); i++)
            {
                object obj2 = pkValues[i];
                str = DbCommon.ParamKey + "ID" + index;
                sql.Append(str).Append(",");
                param[index] = new SqlParam(str, obj2);
                index++;
            }
            str = DbCommon.ParamKey + "ID" + index;
            sql.Append(str);
            param[index] = new SqlParam(str, pkValues[index]);
            sql.Append(")");
            return db.ExecuteBySql(sql, param);
        }
        #endregion
      
    }
}
