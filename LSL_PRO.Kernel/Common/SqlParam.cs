
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace LSL_PRO.Kernel
{
    /// <summary>
    /// 存放参数【键，键值】

    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class SqlParam
    {

        /// <summary>
        /// 目标字段
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DataType { get; set; }
        /// <summary>
        ///值 
        /// </summary>
        public object FiledValue { get; set; }

        public SqlParam()
        {

        }
        public SqlParam(string _FieldName, object _FiledValue)
            : this(_FieldName, DbType.String, _FiledValue)
        {
        }
        public SqlParam(string _FieldName, DbType _DbType, object _FiledValue)
        {           
            this.FieldName = _FieldName;
            this.DataType = _DbType;
            this.FiledValue = _FiledValue;
        }
    }
}
