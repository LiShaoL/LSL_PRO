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
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LSL_PRO_WEB.Controllers
{
    public class RolesPermissionController : Controller
    {
        RolesPermissionIBLL bll = new RolesPermissionBLL();
        ResultModel Rm = new ResultModel();
        SessionUser su = RequestSession.GetSessionUser();
        #region 返回视图
        [Authentication]
        public ActionResult RolesPermission()
        {
            return View();
        }
        [Authentication]
        public ActionResult AddUser()
        {
            return View();
        }
        [Authentication]
        public ActionResult UpdateUser()
        {
            return View();
        }
        [Authentication]
        public ActionResult Accredit()
        {
            return View();
        }
        [Authentication]
        public ActionResult BtnAccredit()
        {
            return View();
        }
        #endregion

        //获取按钮
        [HttpPost]
        public ActionResult GetBtn()
        {
            string mid = Request["mid"];
            IList list = Common.GetBtnByRole(su.UserId, mid);
            return Json(list);     
        }

        #region 批量添加角色用户
        //批量添加角色用户
        [HttpPost]
        public ActionResult AddRoleUser()
        {
            string roleid = Request["roleid"];
            string uid = Request["uid"];
            DataTable dt = getDatatable(roleid, uid);
            bll.InsertBySqlBulkCopy(dt, "LSL_PRO_UserRole", ref Rm);
            return Json(Rm);
        }
        //将List转换到DataTable
        private DataTable getDatatable(string roleid, string uid)
        {
            string[] uids = uid.Split(',');
            List<LSL_PRO_UserRole> lists = new List<LSL_PRO_UserRole>();
            foreach (string i in uids)
            {
                if (i != "")
                {
                    LSL_PRO_UserRole u = new LSL_PRO_UserRole();
                    u.UserRoleId = CommonHelper.GetGuid;
                    u.UserId = i;
                    u.RoleId = roleid;
                    lists.Add(u);
                }

            }
            DataTable dt = DbReader.ToDataTable(lists);
            return dt;
        }
        //添加用户的列表
        [HttpPost]
        public ActionResult UserGetList()
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParam> paramList = new List<SqlParam>();
            string roleid = Request.QueryString["RoleId"];
            int page = Convert.ToInt32(Request["page"]);
            int rows = Convert.ToInt32(Request["rows"]);
            string sidx = Request["sidx"];
            string sord = Request["sord"];
            string strname = Request["strname"];
            string str = Request["str"];
            if (!string.IsNullOrEmpty(strname) && !string.IsNullOrEmpty(str))
            {
                switch (strname)
                {
                    case "Account":
                        sql.Append(" And Account=@Account");
                        paramList.Add(new SqlParam("@Account", str));
                        break;
                    case "RealName":
                        sql.Append(" And RealName like @RealName");
                        paramList.Add(new SqlParam("@RealName", "%" + str + "%"));
                        break;
                    case "Mobile":
                        sql.Append(" And Mobile= @Mobile");
                        paramList.Add(new SqlParam("@Mobile", str));
                        break;
                }

            }
            int count = 0;
            IList list = bll.GetAddUserList(sql, paramList.ToArray(), page, rows, sidx, sord, ref count, roleid);
            var jsonData = JqGridModel.GridData(page, rows, count, list);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 批量删除用户
        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRoleUser()
        {
            string uid = Request["uid"];
            string[] uids = uid.Split(',');
            string[] newArr = new string[uids.Length - 1];
            Array.Copy(uids, 0, newArr, 0, uids.Length - 1);
            bll.DeleteUserByRole(newArr, ref Rm);
            return Json(Rm);
        }
        /// <summary>
        /// 编辑用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserUpdateGetList()
        {
            StringBuilder sql = new StringBuilder();
            List<SqlParam> paramList = new List<SqlParam>();
            string roleid = Request.QueryString["RoleId"];
            int page = Convert.ToInt32(Request["page"]);
            int rows = Convert.ToInt32(Request["rows"]);
            string sidx = Request["sidx"];
            string sord = Request["sord"];
            string strname = Request["strname"];
            string str = Request["str"];
            if (!string.IsNullOrEmpty(strname) && !string.IsNullOrEmpty(str))
            {
                switch (strname)
                {
                    case "Account":
                        sql.Append(" And Account=@Account");
                        paramList.Add(new SqlParam("@Account", str));
                        break;
                    case "RealName":
                        sql.Append(" And RealName like @RealName");
                        paramList.Add(new SqlParam("@RealName", "%" + str + "%"));
                        break;
                    case "Mobile":
                        sql.Append(" And Mobile= @Mobile");
                        paramList.Add(new SqlParam("@Mobile", str));
                        break;
                }

            }
            int count = 0;
            IList list = bll.GetUpdateUserList(sql, paramList.ToArray(), page, rows, sidx, sord, ref count, roleid);
            var jsonData = JqGridModel.GridData(page, rows, count, list);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 获取菜单权限列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJstree()
        {

            IList list = bll.GetJstree();
            return Json(list);
        }
        /// <summary>
        /// 添加角色菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAccredit()
        {
            string roleid = Request["roleid"];
            string mid = Request["mid"];
            DataTable dt = getAccreditTB(roleid, mid);
            if (bll.DeleteRoleMenu(roleid))
            {
                bll.InsertBySqlBulkCopy(dt, "LSL_PRO_RoleMenu", ref Rm);
            }           
            return Json(Rm);
        }

        private DataTable getAccreditTB(string roleid, string mid)
        {
            string[] mids = mid.Split(',');
            List<LSL_PRO_RoleMenu> lists = new List<LSL_PRO_RoleMenu>();
            foreach (string i in mids)
            {
                LSL_PRO_RoleMenu u = new LSL_PRO_RoleMenu();
                u.RoleMenuId = CommonHelper.GetGuid;
                u.RoleId = roleid;
                u.MenuId = i;
                lists.Add(u);

            }
            DataTable dt = DbReader.ToDataTable(lists);
            return dt;
        }

        /// <summary>
        /// 获取已有权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoleMenu()
        {
            string roleid = Request["roleid"];
            IList list = bll.GetRoleMenu(roleid);
            return Json(list);
        }
        [HttpPost]
        public ActionResult GetMenuJstree()
        {
            string roleid = Request["roleid"];
            IList list = bll.GetMenuJstree(roleid);
            return Json(list);
        }
        [HttpPost]
        public ActionResult GetMenuBtn()
        {
            string mid = Request["mid"];
            IList list = bll.GetMenuBtn(mid);
            return Json(list);
        }

        /// <summary>
        /// 菜单按钮授权
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveRoleMenuBtn() 
        {
            string roleid = Request["roleid"];
            string mid = Request["mid"];
            string btnid = Request["btnid"];
            DataTable dt = getRoleBtnTB(roleid, mid,btnid);
            if (bll.DeleteRoleMenuButton(roleid,mid))
            {
                 bll.InsertBySqlBulkCopy(dt, "LSL_PRO_RoleMenuButton", ref Rm);
            }          
            return Json(Rm);
        }

        private DataTable getRoleBtnTB(string roleid, string mid, string btnid)
        {
            string[] btnids = btnid.Split(',');
            List<LSL_PRO_RoleMenuButton> lists = new List<LSL_PRO_RoleMenuButton>();
            foreach (string i in btnids)
            {
                LSL_PRO_RoleMenuButton u = new LSL_PRO_RoleMenuButton();
                u.RoleMenuButtonId = CommonHelper.GetGuid;
                u.RoleId = roleid;
                u.MenuId = mid;
                u.ButtonId = i;
                lists.Add(u);

            }
            DataTable dt = DbReader.ToDataTable(lists);
            return dt;
        }

        public ActionResult GetRoleMenuBtn()
        {
            string roleid = Request["roleid"];
            string mid = Request["mid"];
            IList list = bll.GetRoleMenuBtn(roleid,mid);
            return Json(list);
        }
    }
}