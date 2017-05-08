 
// 
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using System.Data;
using System.Reflection;
using LSL_PRO.Kernel;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// Control 控件绑定帮助类
    /// </summary>
    public class ControlBindHelper
    {
        #region 数据源绑定
        /// <summary>
        /// 绑定GridView DataTable
        /// </summary>
        /// <param name="Columns">数据</param>
        public static void BindGridViewList(DataTable table, GridView grid)
        {
            if (table == null || table.Rows.Count == 0)
            {
                table = table.Clone();
                table.Rows.Add(table.NewRow());
                grid.DataSource = table;
                grid.DataBind();
                int columnCount = table.Columns.Count;
                grid.Rows[0].Cells.Clear();
                grid.Rows[0].Cells.Add(new TableCell());
                grid.Rows[0].Cells[0].ColumnSpan = columnCount;
                grid.Rows[0].Cells[0].Text = "没有找到您要的相关数据";
                grid.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
            else
            {
                grid.DataSource = table;
                grid.DataBind();
            }
        }
        /// <summary>
        /// 绑定IList:GridView 
        /// </summary>
        /// <param name="Columns">数据</param>
        public static void BindGridViewList(IList list, GridView grid)
        {
            grid.DataSource = list;
            grid.DataBind();
        }
        /// <summary>
        /// 绑定DataTable:Repeater控件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        public static void BindRepeaterList(DataTable dt, Repeater repeater)
        {
            repeater.DataSource = dt;
            repeater.DataBind();
        }
        /// <summary>
        /// 绑定IList:Repeater控件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        public static void BindRepeaterList(IList list, Repeater repeater)
        {
            repeater.DataSource = list;
            repeater.DataBind();
        }
        /// <summary>
        /// 绑定IList<T>:Repeater控件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        public static void BindRepeaterList<T>(IList<T> list, Repeater repeater)
        {
            repeater.DataSource = list;
            repeater.DataBind();
        }
        /// <summary>
        /// 绑定DataTable:DropDownList下拉列表框
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        public static void BindDropDownList(DataTable dt, DropDownList dropdownlists, string _Name, string _ID, string _Memo)
        {
            dropdownlists.DataSource = dt;
            dropdownlists.DataTextField = _Name;
            dropdownlists.DataValueField = _ID;
            dropdownlists.DataBind();
            if (!string.IsNullOrEmpty(_Memo.Trim()))
            {
                dropdownlists.Items.Insert(0, new ListItem(_Memo, ""));
            }
        }

        /// <summary>
        /// 绑定IList:DropDownList下拉列表框
        /// </summary>
        /// <param name="list">IList</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        public static void BindDropDownList(IList list, DropDownList dropdownlists, string _Name, string _ID, string _Memo)
        {
            dropdownlists.DataSource = list;
            dropdownlists.DataTextField = _Name;
            dropdownlists.DataValueField = _ID;
            dropdownlists.DataBind();
            if (!string.IsNullOrEmpty(_Memo.Trim()))
            {
                dropdownlists.Items.Insert(0, new ListItem(_Memo, ""));
            }
        }

        /// <summary>
        /// 绑定IList<T>:DropDownList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dl"></param>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public static void BindDropDownList<T>(IList<T> list, DropDownList dropdownlists, string _Name, string _ID, string _Memo)
        {
            dropdownlists.DataSource = list;
            dropdownlists.DataTextField = _Name;
            dropdownlists.DataValueField = _ID;
            dropdownlists.DataBind();
            if (!string.IsNullOrEmpty(_Memo.Trim()))
            {
                dropdownlists.Items.Insert(0, new ListItem(_Memo, ""));
            }
        }

        /// <summary>
        /// 绑定DataTable:HtmlSelect下拉列表框 
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        public static void BindHtmlSelect(DataTable dt, HtmlSelect select, string _Name, string _ID, string _Memo)
        {
            select.DataSource = dt;
            select.DataTextField = _Name;
            select.DataValueField = _ID;
            select.Attributes.Add("title", "123");
            select.DataBind();
            if (!string.IsNullOrEmpty(_Memo.Trim()))
            {
                select.Items.Insert(0, new ListItem(_Memo, ""));
            }
        }
        /// <summary>
        /// 绑定IList:HtmlSelect下拉列表框 
        /// Modify By wanghe 2014-5-20
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        public static void BindHtmlSelect(IList list, HtmlSelect select, string _Name, string _ID, string _Memo)
        {
            BindHtmlSelect(list, select, _Name, _ID, _Memo, "");
        }
        /// <summary>
        /// 绑定IList<T>:HtmlSelect下拉列表框 
        /// Modify By wanghe 2014-5-20
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        /// <param name="defaultValue">默认选中值</param>
        public static void BindHtmlSelect(IList list, HtmlSelect select, string _Name, string _ID, string _Memo, string defaultValue)
        {
            select.DataSource = list;
            select.DataTextField = _Name;
            select.DataValueField = _ID;
            select.DataBind();
            if (!String.IsNullOrEmpty(defaultValue))
                select.Value = defaultValue;

            if (!string.IsNullOrEmpty(_Memo.Trim()))
            {
                select.Items.Insert(0, new ListItem(_Memo, ""));
            }
        }

        /// <summary>
        /// 绑定IList<T>:HtmlSelect下拉列表框 
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        public static void BindHtmlSelect<T>(IList<T> list, HtmlSelect select, string _Name, string _ID, string _Memo)
        {
            select.DataSource = list;
            select.DataTextField = _Name;
            select.DataValueField = _ID;
            select.DataBind();
            if (!string.IsNullOrEmpty(_Memo.Trim()))
            {
                select.Items.Insert(0, new ListItem(_Memo, ""));
            }
        }
        /// <summary>
        /// 绑定IList<T>:RadioButtonList单选框
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="dropdownlists">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>
        /// <param name="_Memo">默认显示值</param>
        public static void BindRadioButtonList(IList list, RadioButtonList rbllist, string _Name, string _ID)
        {
            rbllist.DataSource = list;
            rbllist.DataTextField = _Name;
            rbllist.DataValueField = _ID;
            rbllist.DataBind();
        }
        /// <summary>
        /// 绑定DataTable:RadioButtonList单选框
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rbllist">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>        
        //public static void BindRadioButtonList(DataTable dt, RadioButtonList rbllist, string _Name, string _ID)
        //{
        //    rbllist.DataSource = dt;
        //    rbllist.DataTextField = _Name;
        //    rbllist.DataValueField = _ID;
        //    rbllist.DataBind();
        //}
        /// <summary>
        /// 绑定DataTable:CheckBoxList复选框
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rbllist">控件名称</param>
        /// <param name="_Name">绑定字段名称</param>
        /// <param name="_ID">绑定字段主键</param>        
        public static void BindCheckBoxList(DataTable dt, CheckBoxList checkList, string _Name, string _ID)
        {
            checkList.DataSource = dt;
            checkList.DataTextField = _Name;
            checkList.DataValueField = _ID;
            checkList.DataBind();
        }
        #endregion

        #region GET/SET aspx页面 服务器控件值
        /// <summary>
        /// 获取aspx页面 服务器控件值，返回实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        public static T GetWebControls<T>(Control page)
        {
            Hashtable ht = GetWebControls(page);
            T model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            //遍历每一个属性
            foreach (PropertyInfo prop in type.GetProperties())
            {
                object value = ht[prop.Name];
                if (prop.PropertyType.ToString() == "System.Nullable`1[System.DateTime]")
                {
                    value = CommonHelper.ToDateTime(value);
                }
                prop.SetValue(model, DbReader.HackType(value, prop.PropertyType), null);
            }
            return model;
        }

        /// <summary>
        /// 获取aspx页面 服务器控件值，返回哈希表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static Hashtable GetWebControls(Control page)
        {
            Hashtable ht = new Hashtable();
            int size = HttpContext.Current.Request.Params.Count;
            for (int i = 0; i < size; i++)
            {
                string id = HttpContext.Current.Request.Params.GetKey(i);
                Control control = page.FindControl(id);
                if (control == null) continue;
                control = page.FindControl(id);
                if (control is HtmlInputText)
                {
                    HtmlInputText txt = (HtmlInputText)control;
                    ht[txt.ID] = txt.Value.Trim();
                }
                if (control is HtmlSelect)
                {
                    HtmlSelect txt = (HtmlSelect)control;
                    ht[txt.ID] = txt.Value.Trim();
                }
                if (control is HtmlInputHidden)
                {
                    HtmlInputHidden txt = (HtmlInputHidden)control;
                    ht[txt.ID] = txt.Value.Trim();
                }
                if (control is HtmlInputPassword)
                {
                    HtmlInputPassword txt = (HtmlInputPassword)control;
                    ht[txt.ID] = txt.Value.Trim();
                }
                if (control is HtmlInputCheckBox)
                {
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)control;
                    ht[chk.ID] = chk.Checked ? 1 : 0;
                }
                if (control is HtmlTextArea)
                {
                    HtmlTextArea area = (HtmlTextArea)control;
                    ht[area.ID] = area.Value.Trim();
                }
            }
            return ht;
        }

        /// <summary>
        /// 实体类给服务器控件赋值
        /// </summary>
        /// <param name="page"></param>
        /// <param name="entity">实体类</param>
        public static void SetWebControls<T>(Page page, T entity)
        {
            SetWebControlsWithValue(page, HashtableHelper.GetModelToHashtable(entity), string.Empty, false);
        }

        /// <summary>
        /// 实体类给服务器控件赋值
        /// </summary>
        /// <param name="page">页面</param>
        /// <param name="entity">实体类</param>
        /// <param name="master">模版页的前缀</param>
        public static void SetWebControls<T>(Page page, T entity, string master)
        {
            SetWebControlsWithValue(page, HashtableHelper.GetModelToHashtable(entity), master, false);
        }

        public static void SetWebControlsViewStatus<T>(Page page, T entity, string master)
        {
            SetWebControlsWithValue(page, HashtableHelper.GetModelToHashtable(entity), master, true);
        }

        /// <summary>
        /// 创建哈希表给服务器控件赋值
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ht"></param>
        /// <param name="master"></param>
        public static void SetWebControlsWithValue(Page page, Hashtable ht, string master, bool Disabled)
        {
            //style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none"
            if (ht.Count > 0)
            {
                int size = ht.Keys.Count;
                foreach (string key in ht.Keys)
                {
                    var val = ht[key];
                    //if (val != null)
                    //{
                    Control control;
                    if (string.IsNullOrEmpty(master))
                        control = page.FindControl(key);
                    else
                        control = page.Master.FindControl(master).FindControl(key);

                    if (control == null) continue;
                    if (control is HtmlInputText)
                    {
                        var txt = (HtmlInputText)control;
                        txt.Value = val == null ? "" : val.ToString().Trim();
                        if (Disabled)
                        {
                            txt.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
                            txt.Style.Add(HtmlTextWriterStyle.BackgroundImage, "none");
                            txt.Attributes.Add(HtmlTextWriterAttribute.ReadOnly.ToString(), "true");
                        }
                    }
                    if (control is HtmlSelect)
                    {
                        var sel = (HtmlSelect)control;
                        if (Disabled)
                        {
                            if (val != null && !string.IsNullOrEmpty(val.ToString()))
                            {
                                Control c = new Control();
                                if (string.IsNullOrEmpty(master))
                                    c = page.FindControl("lbl" + key);
                                else
                                    c = page.Master.FindControl(master).FindControl("lbl" + key);
                                Label selLabel = (Label)c;
                                if (selLabel != null)
                                {
                                    if (sel.Items.Count > 0)
                                        selLabel.Text = sel.Items.FindByValue(val.ToString()).Text;
                                    selLabel.Visible = true;
                                }
                            }
                            sel.Visible = false;
                        }
                        else
                            sel.Value = val == null ? "" : val.ToString().Trim();
                    }
                    if (control is HtmlInputHidden)
                    {
                        var txt = (HtmlInputHidden)control;
                        txt.Value = val == null ? "" : val.ToString().Trim();
                    }
                    if (control is HtmlInputPassword)
                    {
                        var txt = (HtmlInputPassword)control;
                        txt.Value = val == null ? "" : val.ToString().Trim();
                    }
                    if (control is Label)
                    {
                        var txt = (Label)control;
                        txt.Text = val == null ? "" : val.ToString().Trim().Replace("<", "&lt;").Replace(">", "&gt;");
                    }
                    if (control is HtmlInputCheckBox)
                    {
                        var chk = (HtmlInputCheckBox)control;
                        chk.Checked = val == null ? false : CommonHelper.GetInt(val) == 1;
                        if (Disabled)
                        {
                            chk.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
                            chk.Style.Add(HtmlTextWriterStyle.BackgroundImage, "none");
                            chk.Attributes.Add(HtmlTextWriterAttribute.Disabled.ToString(), "true");
                        }
                    }
                    if (control is HtmlTextArea)
                    {
                        var area = (HtmlTextArea)control;
                        area.Value = val == null ? "" : val.ToString().Trim();
                        if (Disabled)
                        {
                            area.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");
                            area.Style.Add(HtmlTextWriterStyle.BackgroundImage, "none");
                            area.Attributes.Add(HtmlTextWriterAttribute.ReadOnly.ToString(), "true");
                        }
                    }

                    // }
                }
            }
        }

        #endregion

        #region 动态生成表单
        /// <summary>
        /// 返回控件属性
        /// </summary>
        /// <param name="Control_ID">控件ID</param>
        /// <param name="Name">属性名称</param>
        /// <param name="Type">控件类型：1-文本框，2-下拉框，3-日期框，4-标  签，5-文本区</param>
        /// <param name="SourceType">数据源类型：0-固定，1-动态</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="Length">长度</param>
        /// <param name="Height">高度</param>
        /// <param name="Showlength">显示最大长度</param>
        /// <param name="Style">样式</param>
        /// <param name="Validator">控件校验</param>
        /// <param name="Event">触发事件</param>
        /// <returns></returns>
        public static string LoadFormHtml(string Control_ID, string Name, string Type, string SourceType, string DataSource, string Length, string Height, string Showlength, string Style, string Validator, string Event)
        {
            StringBuilder property = new StringBuilder();
            property.Append("<tr>");
            property.Append("<th>" + Name + "：</th>");
            property.Append("<td>");
            property.Append(GetControl_Type(Control_ID, Name, Type, SourceType, DataSource, Length, Height, Showlength, Style, Validator, Event));
            property.Append("</td>");
            property.Append("</tr>");
            return property.ToString();
        }
        /// <summary>
        /// 返回控件类型
        /// </summary>
        /// <param name="Control_ID">控件ID</param>
        /// <param name="Name">属性名称</param>
        /// <param name="Type">控件类型：1-文本框，2-下拉框，3-日期框，4-标  签，5-文本区</param>
        /// <param name="SourceType">数据源类型：0-固定，1-动态</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="Length">长度</param>
        /// <param name="Height">高度</param>
        /// <param name="Showlength">显示最大长度</param>
        /// <param name="Style">样式</param>
        /// <param name="Validator">控件校验</param>
        /// <param name="Event">触发事件</param>
        /// <returns></returns>
        public static string GetControl_Type(string Control_ID, string Name, string Type, string SourceType, string DataSource, string Length, string Height, string Showlength, string Style, string Validator, string Event)
        {
            if (!string.IsNullOrEmpty(Validator.Trim()))
            {
                Validator = "datacol=\"yes\" err=\"" + Name + "\" checkexpession=\"" + Validator + "\"";
            }
            StringBuilder sb = new StringBuilder();
            switch (Type)
            {
                case "1"://1 - 文本框
                    sb.Append("<input id=\"" + Control_ID + "\" " + Event + " maxlength=" + Showlength + " type=\"text\" class=\"" + Style + "\" style=\"width: " + Length + "\" " + Validator + "/>");
                    return sb.ToString();
                case "2"://2 - 下拉框
                    sb.Append("<select id=\"" + Control_ID + "\" " + Event + " class=\"" + Style + "\" style=\"width: " + Length + "\" " + Validator + "/>");
                    if (DataSource.Trim() != "")
                    {
                        string[] strSource = DataSource.Split(';');
                        foreach (string item in strSource)
                        {
                            if (!string.IsNullOrEmpty(item.Trim()))
                            {
                                string[] stritem = item.Split('|');
                                string value = stritem[0];
                                string text = stritem[1];
                                sb.Append("<option value=\"" + value + "\">" + text + "</option>");
                            }
                        }
                    }
                    sb.Append("</select>");
                    return sb.ToString();
                case "3"://3 - 日期框
                    sb.Append("<input id=\"" + Control_ID + "\" " + Event + "  type=\"text\" class=\"" + Style + "\" style=\"width: " + Length + "\" " + Validator + "/>");
                    return sb.ToString();
                case "4"://4 - 标签
                    sb.Append("<label id=\"" + Control_ID + "\"/>");
                    return sb.ToString();
                case "5"://5 - 文本区
                    sb.Append("<textarea id=\"" + Control_ID + "\" " + Event + " maxlength=" + Showlength + " type=\"text\" class=\"" + Style + "\" style=\"width: " + Length + ";height: " + Height + "\" " + Validator + "></textarea>");
                    return sb.ToString();
                default:
                    return "内部错误";
            }
        }
        public string ToSelect(string SourceType, string DataSource)
        {
            return "";
        }
        #endregion
    }
}
