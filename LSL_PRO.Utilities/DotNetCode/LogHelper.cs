 
// 
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Threading;
using LSL_PRO.Kernel;
using LSL_PRO.Utilities;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 日志记录的类型
    /// </summary>
    public enum LogType : int
    {
        /// <summary>
        /// 程序错误
        /// </summary>
        ApplicationError = -1,
        /// <summary>
        /// 系统错误
        /// </summary>
        SysError,
        /// <summary>
        /// 常规操作
        /// </summary>
        Common,
        /// <summary>
        /// 安装操作
        /// </summary>
        Setup,
        /// <summary>
        /// 登陆
        /// </summary>
        Login,
        /// <summary>
        /// 退出
        /// </summary>
        Logout,
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown,
        /// <summary>
        /// 无
        /// </summary>
        None
    }
    /// <summary>
    ///  企业应用框架的日志类
    /// </summary>    
    public class LogHelper : IDisposable
    {
        private string LogFile;
        private static string _LogFilePath;
        private static object _lock = new object();
        private static LogHelper _instance;
        string logIsWrite = ConfigHelper.GetValue("LogIsWrite");

        public static LogHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LogHelper();

                return _instance;
            }
        }
        /// <summary>
        /// 实例日志管理，以当前日期为文件名，如果文件不存在，则创建文件
        /// </summary>
        private LogHelper()
        {
            CreateLoggerFile(null);
        }
        /// <summary>
        /// 实例日志管理，如果文件不存在，则创建文件
        /// </summary>
        /// <param name="_log">创建txt文件名称</param>
        public LogHelper(string _log)
        {
            CreateLoggerFile(_log);
        }

        private static string LogFilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_LogFilePath))
                    _LogFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString() + "//";//System.Web.HttpContext.Current.Server.MapPath("~/Error/SysLog/");

                if (!Directory.Exists(_LogFilePath))
                    Directory.CreateDirectory(_LogFilePath);

                return _LogFilePath;
            }
        }

        private void CreateLoggerFile(string fileName)
        {
            if (logIsWrite == "true")//是否写日志
            {
                Object _myLogPath;
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = DateTimeHelper.GetToday();
                }
                _myLogPath = null;
                if (_myLogPath == null)
                {
                    this.LogFile = LogFilePath + DateTimeHelper.GetToday();// ConfigurationManager.AppSettings["LogFilePath"] + "//SysLog//" + DateTimeHelper.GetToday();
                    if (!Directory.Exists(this.LogFile))
                    {
                        Directory.CreateDirectory(this.LogFile);
                    }
                }
                else
                {
                    LogFile = _myLogPath.ToString();
                }
                if (1 > LogFile.Length)
                {
                    Console.WriteLine("配置文件中没有指定日志文件目录！");
                    return;
                }
                if (false == Directory.Exists(LogFile))
                {
                    Console.WriteLine("配置文件中指定日志文件目录不存在！");
                    return;
                }
                if ((LogFile.Substring((LogFile.Length - 1), 1).Equals("/")) || (LogFile.Substring(LogFile.Length - 1, 1).Equals("\\")))
                {
                    LogFile = LogFile + fileName + ".log";
                }
                else
                {
                    LogFile = LogFile + "\\" + fileName + ".log";
                }
                try
                {
                    FileStream fs = new FileStream(LogFile, FileMode.OpenOrCreate);
                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        /// <summary>
        /// 向日志文件中写入日志
        /// </summary>
        /// <param name="messagestr"></param>
        private void WriteInfo(String msg)
        {
            WriteInfo(null, msg);
        }
        /// <summary>
        /// 向日志文件中写入日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void WriteInfo(string title, String msg)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    title = "";

                using (StreamWriter sw = new StreamWriter(LogFile, true))//追加日志
                {
                    sw.WriteLine("******************************" + title + "*****************************************");
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                    sw.WriteLine("输出信息：错误信息");
                    if (msg != null)
                    {
                        sw.WriteLine("异常信息：\r\n" + msg);
                    }
                    sw.Flush();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 写入日志内容
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void WriteLog(string msg)
        {
            if (msg != null)
            {
                WriteInfo(msg.ToString());
            }
        }
        /// <summary>
        /// 写入日志内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg">日志消息</param>
        public void WriteLog(string title, string msg)
        {
            if (msg != null)
            {
                WriteInfo(title, msg.ToString());
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
        }

        #endregion
    }
}
