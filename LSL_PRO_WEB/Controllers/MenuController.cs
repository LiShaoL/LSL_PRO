using LSL_PRO.Kernel;
using LSL_PRO.Utilities;
using LSL_PRO_BLL;
using LSL_PRO_DAL;
using LSL_PRO_IBLL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LSL_PRO_WEB.Controllers
{
    public class MenuController : Controller
    {
        MenuIBLL bll = new MenuBLL();
        ResultModel Rm = new ResultModel();
        SessionUser su = RequestSession.GetSessionUser();
        #region 视图
        [Authentication]
        public ActionResult Menu()
        {
            return View();
        }
        [Authentication]
        public ActionResult MenuAdd()
        {
            return View();
        }
        [Authentication]
        public ActionResult MenuAddBtn()
        {
            return View();
        }
        #endregion

        #region 逻辑
        /// <summary>
        /// 分页获取列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuGetList()
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
        /// 获取上级菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTopMenu()
        {
            IList list = bll.GetTopMune();
            return Json(list);
        }
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInfo()
        {
            string menuID = Request["menuID"];
            string category = Request["category"];
            string topmenu = Request["topmenu"];
            string menuName = Request["menuName"];
            string ImgName = Request["ImgName"];
            string url = Request["url"];
            if (menuID != "")
            {
                Rm = Update(menuID, category, topmenu, menuName, ImgName, url);
            }
            else
            {
                Rm = Insert(category, topmenu, menuName, ImgName, url);
            }
            return Json(Rm);
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Update(string id, string category, string topmenu, string menuName, string ImgName, string url)
        {
            LSL_PRO_Menu men = new LSL_PRO_Menu();
            men.MenuId = id;
            men.ParentId = topmenu;
            men.Category = category;
            men.FullName = menuName;
            men.Img = ImgName;
            men.NavigateUrl = url;
            men.ModifyDate = DateTime.Now;
            bll.Update(men, ref Rm);
            return Rm;
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Insert(string category, string topmenu, string menuName, string ImgName, string url)
        {
            LSL_PRO_Menu men = new LSL_PRO_Menu();
            men.MenuId = CommonHelper.GetGuid;
            if (topmenu == "")
            {
                men.ParentId = "#";
            }
            else
            {
                men.ParentId = topmenu;
            }

            men.Category = category;
            men.FullName = menuName;
            men.Img = ImgName;
            men.NavigateUrl = url;
            men.Enabled = 1;
            men.ModifyDate = DateTime.Now;
            bll.Add(men, ref Rm);
            return Rm;
        }
        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMenuInfo()
        {
            string id = Request["id"];
            bll.GetMenuByID(id, ref Rm);
            return Json(Rm);

        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete()
        {
            string id = Request["id"];
            bll.DeleteByID(id, ref Rm);
            return Json(Rm);

        }

        /// <summary>
        /// 分配按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddMenuBtn()
        {
            string menuID = Request["id"];
            string btnid = Request["btnid"];
            if (bll.DeleteMenuBtnByID(menuID))
            {
                DataTable dt = getDatatable(menuID, btnid);
                bll.AddMenuBtn(dt, "LSL_PRO_MenuButton", ref Rm);
            }
            else 
            {
                Rm.code = "-1";
                Rm.msg = "删除原有按钮失败，请重试";
            }            
            return Json(Rm);
        }
        //将List转换到DataTable
        private DataTable getDatatable(string menuID, string btnid)
        {
            string[] btnids = btnid.Split(',');
            List<LSL_PRO_MenuButton> lists = new List<LSL_PRO_MenuButton>();
            foreach (string i in btnids)
            {
                if (i != "")
                {
                    LSL_PRO_MenuButton m = new LSL_PRO_MenuButton();
                    m.SysMenuButtonId = CommonHelper.GetGuid;
                    m.MenuId = menuID;
                    m.ButtonId = i;
                    lists.Add(m);
                }

            }
            DataTable dt = DbReader.ToDataTable(lists);
            return dt;
        }
        //获取已存在的菜单按钮
        [HttpPost]
        public ActionResult GetMenuBtn()
        {
            string id = Request["id"];
            IList list = bll.GetMenuBtn(id);
            return Json(list);
        }
        #endregion
       
	}
}