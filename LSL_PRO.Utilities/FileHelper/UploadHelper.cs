 
// 
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using LSL_PRO.Utilities;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 文件上传帮助类
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="filleupload">上传文件控件</param>
        /// <param name="filename">文件名</param>
        /// <param name="fileExtension">后缀名</param>
        /// <param name="filesize">文件大小</param>
        /// <returns></returns> 
        public static string FileUpload(FileUpload filleupload, string path, out string filename, out string fileExtension, out string filesize)
        {
            string FileName = CommonHelper.GetGuid;
            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
            //取得文件的扩展名,并转换成小写
            string Extension = System.IO.Path.GetExtension(filleupload.FileName).ToLower();
            fileExtension = Extension;
            filename = FileName + Extension;
            //取得文件大小
            filesize = FileHelper.CountSize(filleupload.PostedFile.ContentLength);
            try
            {
                int Size = filleupload.PostedFile.ContentLength / 1024 / 1024;
                if (Size > 10)
                {
                    return "上传失败,文件过大";
                }
                else
                {
                    filleupload.PostedFile.SaveAs(path + filename);
                    return "上传成功";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WriteLog("-----------错误处理页面-----------\r\n" + ex.ToString() + "\r\n");
                return "上传失败";
            }
        }
        /// <summary>
        /// 文件上传 按原名称
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="filleupload">上传文件控件</param>
        /// <param name="filesize">文件大小</param>
        /// <returns></returns> 
        public static string FileUpload(FileUpload filleupload, string path, out string fileExtension, out string filesize)
        {
            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
            //取得文件的扩展名,并转换成小写
            string Extension = System.IO.Path.GetExtension(filleupload.FileName).ToLower();
            fileExtension = Extension;
            //取得文件大小
            filesize = FileHelper.CountSize(filleupload.PostedFile.ContentLength);
            try
            {
                int Size = filleupload.PostedFile.ContentLength / 1024 / 1024;
                if (Size > 10)
                {
                    return "上传失败,文件过大";
                }
                else
                {
                    filleupload.PostedFile.SaveAs(path + filleupload.FileName);
                    return "上传成功";
                }
            }
            catch (Exception)
            {
                return "上传失败";
            }
        }
    }
}
