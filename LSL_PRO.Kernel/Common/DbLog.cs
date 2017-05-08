 
// 
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LSL_PRO.Kernel
{
    /// <summary>
    /// 在本地写入错误日志

    /// <author>
    ///		<name>lsl</name>
    ///		<date>2016.11.20</date>
    /// </author>
    /// </summary>
    public class DbLog
    {
        /// <summary>
        /// 只读对象用于锁
        /// </summary>
        private static readonly object writeFile = new object();
        private static StreamWriter streamWriter; //写文件  

        public static void WriteException(Exception exception)
        {
            WriteLog(exception);
        }
        /// <summary>
        /// 在本地写入错误日志
        /// </summary>
        /// <param name="exception"></param> 错误信息
        public static void WriteLog(Exception exception)
        {
            lock (writeFile)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    string directPath = ConfigHelper.GetValue("LogFilePath") + "\\DbLog";
                    //记录错误日志文件的路径
                    if (!Directory.Exists(directPath))
                    {
                        Directory.CreateDirectory(directPath);
                    }
                    directPath += string.Format(@"\{0}.log", dt.ToString("yyyy-MM-dd"));
                    if (streamWriter == null)
                    {
                        InitLog(directPath);
                    }
                    streamWriter.WriteLine("***********************************************************************");
                    streamWriter.WriteLine(dt.ToString("HH:mm:ss"));
                    streamWriter.WriteLine("输出信息：错误信息");
                    if (exception != null)
                    {
                        DbErrorMsg.ReturnMsg = exception.Message;

                        streamWriter.WriteLine("异常信息：\r\n" + exception.Message);
                        streamWriter.WriteLine("堆栈信息：\r\n" + exception.StackTrace);

                    }
                }
                finally
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Flush();
                        streamWriter.Dispose();
                        streamWriter = null;
                    }
                }
            }
        }
        private static void InitLog(string filPath)
        {
            streamWriter = !File.Exists(filPath) ? File.CreateText(filPath) : File.AppendText(filPath);
        }
    }
}
