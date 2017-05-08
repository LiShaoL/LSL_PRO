
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;

namespace LSL_PRO.Kernel
{

    /// <summary>
    /// 利用反射实现通用的DataReader转List、DataReader转实体类 
    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class DbReader
    {
        #region 利用反射实现通用的DataReader转List、DataReader转实体类
        /// <summary>
        ///  将IDataReader转换为DataTable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DataTable ReaderToDataTable(IDataReader reader)
        {
            DataTable objDataTable = new DataTable("Table");
            int intFieldCount = reader.FieldCount;
            for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
            {
                objDataTable.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
            }
            objDataTable.BeginLoadData();
            object[] objValues = new object[intFieldCount];
            while (reader.Read())
            {
                reader.GetValues(objValues);
                objDataTable.LoadDataRow(objValues, true);
            }
            reader.Close();
            objDataTable.EndLoadData();
            return objDataTable;
        }
        #endregion

        #region 将IDataReader转换为 集合
        /// <summary>
        /// 将IDataReader转换为 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static List<T> ReaderToList<T>(IDataReader dr)
        {

            using (dr)
            {
                List<string> field = new List<string>(dr.FieldCount);
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    field.Add(dr.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (dr.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (field.Contains(property.Name.ToLower()))
                        {
                            if (!IsNullOrDBNull(dr[property.Name]))
                            {
                                try
                                {
                                    property.SetValue(model, HackType(dr[property.Name], property.PropertyType), null);
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }
        #endregion

        #region DataTable 转 泛型集合IList
        /// <summary>
        /// DataTable 转 泛型集合IList
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>行中是对象的类，类的属性与数据字段一致</returns>
        public static IList DataTableToIList<T>(DataTable dt)
        {
            // 定义集合    
            IList list = new List<T>();
            // 获得此模型的类型    
            // Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                //T t = new T();
                T obj = Activator.CreateInstance<T>();
                // 获得此模型的公共属性    
                PropertyInfo[] propertys = obj.GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter    
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(obj, value, null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion

        #region 将泛型集合IList 转换成DataTable
        /// <summary>    
        /// 将泛型集合类转换成DataTable    
        /// </summary>    
        /// <typeparam name="T">集合项类型</typeparam>    
        /// <param name="list">集合</param>    
        /// <param name="propertyName">需要返回的列的列名</param>    
        /// <returns>数据集(表)</returns>    
        //public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
        //{
        //    List<string> propertyNameList = new List<string>();
        //    if (propertyName != null)
        //        propertyNameList.AddRange(propertyName);
        //    DataTable result = new DataTable();
        //    if (list.Count > 0)
        //    {
        //        PropertyInfo[] propertys = list[0].GetType().GetProperties();
        //        foreach (PropertyInfo pi in propertys)
        //        {
        //            if (propertyNameList.Count == 0)
        //            {
        //                result.Columns.Add(pi.Name, pi.PropertyType);
        //            }
        //            else
        //            {
        //                if (propertyNameList.Contains(pi.Name))
        //                    result.Columns.Add(pi.Name, pi.PropertyType);
        //            }
        //        }

        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            ArrayList tempList = new ArrayList();
        //            foreach (PropertyInfo pi in propertys)
        //            {
        //                if (propertyNameList.Count == 0)
        //                {
        //                    object obj = pi.GetValue(list[i], null);
        //                    tempList.Add(obj);
        //                }
        //                else
        //                {
        //                    if (propertyNameList.Contains(pi.Name))
        //                    {
        //                        object obj = pi.GetValue(list[i], null);
        //                        tempList.Add(obj);
        //                    }
        //                }
        //            }
        //            object[] array = tempList.ToArray();
        //            result.LoadDataRow(array, true);
        //        }
        //    }
        //    return result;
        //}
        /// <summary>
        /// 将泛型集合类转换成DataTable    
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            PropertyInfo[] oProps = null;
            foreach (T rec in varlist)
            {
                if (oProps == null)
                {

                    oProps = ((Type)rec.GetType()).GetProperties();

                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        #endregion

        #region 将DataRow转换为 实体类
        /// <summary>
        /// 将DataRow转换为 实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ReaderToModel<T>(DataRow dr)
        {

            T model = Activator.CreateInstance<T>();
            foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                //  if()
                if (dr.Table.Columns.Contains(pi.Name) && !IsNullOrDBNull(dr[pi.Name]))
                {
                    pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                }
            }
            return model;
        }
        #endregion

        #region 这个类对可空类型进行判断转换，要不然会报错
        //这个类对可空类型进行判断转换，要不然会报错
        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            if (conversionType == typeof(Int32) && (value == null || value.ToString().Length == 0))
                value = 0;
            if (conversionType == typeof(Double) && (value == null || value.ToString().Length == 0))
                value = 0;
            if (conversionType == typeof(Decimal) && (value == null || value.ToString().Length == 0))
                value = 0;
            return Convert.ChangeType(value, conversionType);
        }
        #endregion

        #region 判断是否是null或DBnull
        //判断是否是null或DBnull
        public static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }
        #endregion



    }
}
