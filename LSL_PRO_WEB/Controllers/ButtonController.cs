using LSL_PRO.Kernel;
using LSL_PRO.Utilities;
using LSL_PRO_BLL;
using LSL_PRO_DAL;
using LSL_PRO_IBLL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LSL_PRO_WEB.Controllers
{
    public class ButtonController : Controller
    {
        ButtonIBLL bll = new ButtonBLL();
        ResultModel Rm = new ResultModel();
        SessionUser su = RequestSession.GetSessionUser();
        [Authentication]
        public ActionResult Button()
        {
            return View();
        }
        [Authentication]
        public ActionResult BtnAdd()
        {
            return View();
        }
        /// <summary>
        /// 分页获取列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ButtonGetList()
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParam> paramList = new List<SqlParam>();
            int page = Convert.ToInt32(Request["page"]);
            int rows = Convert.ToInt32(Request["rows"]);
            string sidx = Request["sidx"];
            string sord = Request["sord"];
            string strname = Request["strname"];
            string str = Request["str"];
            if (!string.IsNullOrEmpty(strname) && !string.IsNullOrEmpty(str))
            {
                sql.Append(" And FullName like @FullName");
                paramList.Add(new SqlParam("@FullName", "%" + str + "%"));               
            }
            int count = 0;
            IList list = bll.GetList(sql, paramList.ToArray(), page, rows, sidx, sord, ref count);
            var jsonData = JqGridModel.GridData(page, rows, count, list);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        //获取按钮
        [HttpPost]
        public ActionResult GetBtn()
        {
            string mid = Request["mid"];
            IList list = Common.GetBtnByRole(su.UserId, mid);
            return Json(list); 
        }
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInfo()
        {
            string BtnID = Request["BtnID"];
            string BtnName = Request["BtnName"];
            string ImgName = Request["ImgName"];
            string Event = Request["Event"];
            string CSS = Request["CSS"];
            if (BtnID!="")
            {
                Rm = Update(BtnID, BtnName, ImgName, Event,CSS);
            }
            else
            {
                Rm = Insert( BtnName, ImgName, Event, CSS);
            }
            return Json(Rm);
        }
        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Update(string id, string name, string img, string ff,string css)
        {
            LSL_PRO_Button btn = new LSL_PRO_Button();
            btn.ButtonId = id;
            btn.FullName = name;
            btn.Img = img;
            btn.Event = ff;
            btn.Description = css;
            btn.Enabled = 1;
            bll.UpdateBtn(btn, ref Rm);
            return Rm;
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Insert( string name, string img, string ff, string css)
        {
            LSL_PRO_Button btn = new LSL_PRO_Button();
            btn.ButtonId = CommonHelper.GetGuid;
            btn.FullName = name;
            btn.Img = img;
            btn.Event = ff;
            btn.Description = css;
            btn.Enabled = 1;
            bll.AddButton(btn, ref Rm);
            return Rm;
        }
        /// <summary>
        /// 获取按钮信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetButtonInfo()
        {
            string id = Request["id"];
            bll.GetButtonByID(id, ref Rm);
            return Json(Rm);

        }
        [HttpPost]
        public ActionResult Delete()
        {
            string id = Request["id"];
            bll.DeleteByID(id, ref Rm);
            return Json(Rm);

        }
	}
}