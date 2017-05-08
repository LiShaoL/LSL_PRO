using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using System.IO;
using Aspose.Words.Tables;
using System.Data;

namespace LSL_PRO.Utilities.AtlasPolaris.DocumentHandle
{
    /// <summary>
    /// Aspose.Words.dll文档操作帮助类
    /// </summary>
    public class AsposeWordHelper
    {
        private Document doc = null;
        private DocumentBuilder builder = null;


        public void OpenWordDocument(string docPath)
        {
            doc = new Document(docPath);
            builder = new Aspose.Words.DocumentBuilder(doc);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            doc = null;
            builder = null;
        }

        public bool ReplaceBookmarks(Dictionary<string, string> dictSource)
        {
            if (dictSource == null || dictSource.Count == 0) return false;
            StringBuilder temp = new StringBuilder();
            foreach (string name in dictSource.Keys)
            {
                temp.Clear();
                temp.Append(string.IsNullOrEmpty(dictSource[name]) ? "" : dictSource[name]);
                ReplaceBookmark(name, dictSource[name]);
            }
            return true;
        }

        public void ReplaceBookmark(string markname, string value)
        {
            value = string.IsNullOrEmpty(value) ? "" : value;
            Bookmark bm = doc.Range.Bookmarks[markname];
            if (bm != null)
                bm.Text = value;
        }

        /// <summary>
        /// 在标签处插入表格
        /// </summary>
        /// <param name="nameList"></param>
        /// <param name="mark"></param>
        public void InsertTable(DataTable nameList, string markName, int docTableIndex)
        {
            try
            {
                List<double> widthList = new List<double>();
                for (int i = 0; i < nameList.Columns.Count; i++)
                {
                    builder.MoveToCell(docTableIndex, 0, i, 0); //移动单元格
                    double width = builder.CellFormat.Width;//获取单元格宽度
                    widthList.Add(width);
                }

                builder.MoveToBookmark(markName);        //开始添加值
                for (var i = 0; i < nameList.Rows.Count; i++)
                {
                    for (var j = 0; j < nameList.Columns.Count; j++)
                    {
                        builder.InsertCell();// 添加一个单元格                    
                        builder.CellFormat.Borders.LineStyle = LineStyle.Single;
                        builder.CellFormat.Borders.Color = System.Drawing.Color.Black;
                        builder.CellFormat.Width = widthList[j];
                        builder.CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.None;
                        builder.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;

                        //builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;//垂直居中对齐
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;//水平居中对齐
                        builder.Write(nameList.Rows[i][j].ToString());
                    }
                    builder.EndRow();
                }
                doc.Range.Bookmarks[markName].Text = "";    //清除标识  

                //if (MessageUtil.ShowYesNoAndTips("保存成功，是否打开文件？") == System.Windows.Forms.DialogResult.Yes)
                //{
                //    System.Diagnostics.Process.Start(saveDocFile);
                //}
            }
            catch (Exception ex)
            {
                return;
            }
        }


        public bool Save(string filePath)
        {
            doc.Save(filePath);
            return File.Exists(filePath);
        }

        public bool Save(System.Web.HttpResponse Response, string filePath)
        {
            doc.Save(Response, filePath, Aspose.Words.ContentDisposition.Attachment,
                Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            return File.Exists(filePath);
        }

        public bool SaveAsPDF(string filePath)
        {
            if (doc != null)
            {
                //Aspose.Words.Saving.SaveOutputParameters sop = doc.Save(filePath, SaveFormat.Pdf,);
                doc.Save(filePath, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Pdf));
            }
            return File.Exists(filePath);
        }

        public static bool SaveAsFormat(string wordFilePath, string formatFilePath, SaveFormat format)
        {
            bool result = false;
            Document awd;
            try
            {
                awd = new Document(wordFilePath);
                awd.Save(formatFilePath, format);
            }
            catch { }
            finally { awd = null; }
            result = File.Exists(formatFilePath);
            return result;
        }
    }
}
