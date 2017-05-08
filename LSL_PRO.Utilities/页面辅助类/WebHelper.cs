 
// 
 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections;
using System.Web.UI.WebControls;
using System.Threading;
using LSL_PRO.Kernel;

namespace LSL_PRO.Utilities
{
    /// <summary>
    /// Web帮助类
    /// </summary>
    public class WebHelper
    {
        #region 防止刷新重复提交
        /// <summary>
        /// 防止刷新重复提交
        /// </summary>
        /// <param name="btn">按钮控件</param>
        /// <returns></returns>
        public static bool SubmitCheckForm(LinkButton btn)
        {
            if (HttpContext.Current.Request.Form.Get("txt_hiddenToken").Equals(GetToken()))
            {
                SetToken();
                Thread.Sleep(500);////延迟500毫秒
                return true;
            }
            else
            {
                ShowMsgHelper.showWarningMsg("为了保证表单不重复提交，提交无效");
                return false;
            }
        }
        /// <summary>
        /// 获得当前Session里保存的标志
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            HttpContext rq = HttpContext.Current;
            if (null != rq.Session["Token"])
            {
                return rq.Session["Token"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 生成标志，并保存到Session
        /// </summary>
        public static void SetToken()
        {
            HttpContext rq = HttpContext.Current;
            rq.Session.Add("Token", Md5Helper.MakeMd5(rq.Session.SessionID + DateTime.Now.Ticks.ToString()));
        }
        #endregion

        #region 清理浏览器缓存

        /// <summary>
        /// 清理浏览器缓存
        /// </summary>
        public static void ClearBrowerCache()
        {
            try
            {
                //清理IE缓存
                CommonHelper.Run("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
                //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            catch (Exception ex) { }

        }
        #endregion
    }
}
