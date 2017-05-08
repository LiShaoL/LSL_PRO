 
// 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 导出Word帮助类
    /// </summary>
    public class WordHelper
    {
        /// <summary>
        /// 创建系统异常日志
        /// </summary>
        protected static LogHelper Logger = new LogHelper("WordHelper");
        /// <summary>
        /// 导出Word
        /// </summary>
        /// <param name="sbHtml">html标签</param>
        /// <param name="fileName">文件名</param>
        public static void ExportWord(StringBuilder sbHtml, string fileName)
        {
            try
            {
                if (sbHtml.Length > 0)
                {
                    HttpContext.Current.Response.ContentType = "application/ms-word";
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    HttpContext.Current.Response.Charset = "Utf-8";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".doc", System.Text.Encoding.UTF8));
                    HttpContext.Current.Response.Write(sbHtml.ToString());
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.ToString());
            }
        }
        /// <summary>  
        /// 把Word文件转换成为PDF格式文件  
        /// </summary>  
        /// <param name="sourcePath">源文件路径</param>  
        /// <param name="targetPath">目标文件路径</param>   
        /// <returns>true=转换成功</returns>  
        //public static bool Convert(string sourcePath, string targetPath, Microsoft.Office.Interop.Word.WdExportFormat exportFormat)
        //{
        //    bool result;
        //    object paramMissing = Type.Missing;
        //    Microsoft.Office.Interop.Word.ApplicationClass wordApplication = new Microsoft.Office.Interop.Word.ApplicationClass();
        //    Microsoft.Office.Interop.Word.Document wordDocument = null;
        //    try
        //    {
        //        object paramSourceDocPath = sourcePath;
        //        string paramExportFilePath = targetPath;

        //        Microsoft.Office.Interop.Word.WdExportFormat paramExportFormat = exportFormat;
        //        bool paramOpenAfterExport = false;
        //        Microsoft.Office.Interop.Word.WdExportOptimizeFor paramExportOptimizeFor =
        //                Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
        //        Microsoft.Office.Interop.Word.WdExportRange paramExportRange = Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument;
        //        int paramStartPage = 0;
        //        int paramEndPage = 0;
        //        Microsoft.Office.Interop.Word.WdExportItem paramExportItem = Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent;
        //        bool paramIncludeDocProps = true;
        //        bool paramKeepIRM = true;
        //        Microsoft.Office.Interop.Word.WdExportCreateBookmarks paramCreateBookmarks =
        //                Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        //        bool paramDocStructureTags = true;
        //        bool paramBitmapMissingFonts = true;
        //        bool paramUseISO19005_1 = false;

        //        wordDocument = wordApplication.Documents.Open(
        //                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
        //                ref paramMissing, ref paramMissing, ref paramMissing,
        //                ref paramMissing, ref paramMissing, ref paramMissing,
        //                ref paramMissing, ref paramMissing, ref paramMissing,
        //                ref paramMissing, ref paramMissing, ref paramMissing,
        //                ref paramMissing);

        //        if (wordDocument != null)
        //            wordDocument.ExportAsFixedFormat(paramExportFilePath,
        //                    paramExportFormat, paramOpenAfterExport,
        //                    paramExportOptimizeFor, paramExportRange, paramStartPage,
        //                    paramEndPage, paramExportItem, paramIncludeDocProps,
        //                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
        //                    paramBitmapMissingFonts, paramUseISO19005_1,
        //                    ref paramMissing);
        //        result = true;
        //    }
        //    finally
        //    {
        //        if (wordDocument != null)
        //        {
        //            wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
        //            wordDocument = null;
        //        }
        //        if (wordApplication != null)
        //        {
        //            wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
        //            wordApplication = null;
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    return result;
        //}

    }
}
