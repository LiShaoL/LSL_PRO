using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// 这是系统的核心基础信息部分
    /// </summary>
    public class BaseSystemInfo
    {
        /// <summary>
        /// 用户是否已经成功登录系统
        /// </summary>
        public static bool UserIsLogOn = false;

        /// <summary>
        /// 用户在线状态
        /// </summary>
        public static int OnLineState = 0;

        /// <summary>
        /// 当前网站的安装地址
        /// </summary>
        public static string StartupPath = string.Empty;

        /// <summary>
        /// 当前版本
        /// </summary>
        public static string Version = "1.0";

        /// <summary>
        /// 软件名称
        /// </summary>
        public static string SoftName = string.Empty;

        /// <summary>
        /// 软件许可证编号
        /// </summary>
        public static string SoftLicence = string.Empty;

        /// <summary>
        /// 当前客户公司名称
        /// </summary>
        public static string CustomerCompanyName = string.Empty;
    }
}
