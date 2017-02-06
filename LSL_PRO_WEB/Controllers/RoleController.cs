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
    public class RoleController : Controller
    {
        RoleIBLL bll = new RoleBLL();
        ResultModel Rm = new ResultModel();
        SessionUser su = RequestSession.GetSessionUser();
        [Authentication]
        public ActionResult Role()
        {
            return View();
        }
        [Authentication]
        public ActionResult RoleAdd()
        {
            return View();
        }
        /// <summary>
        /// 分页获取列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleGetList()
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
            string id = Request["RoleId"];
            string name = Request["RoleName"];
            string category = Request["lx"];
            if (id != "")
            {
                Rm = Update(id,name,category);
            }
            else
            {
                Rm = Insert(name, category);
            }
            return Json(Rm);
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Update(string id, string name, string category)
        {
            LSL_PRO_Roles rol = new LSL_PRO_Roles();
            rol.RoleId = CommonHelper.GetGuid;
            rol.FullName = name;
            rol.Category = category;
            rol.Enabled = 1;
            rol.CreateDate = DateTime.Now;
            bll.Update(rol, ref Rm);
            return Rm;
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Insert(string name, string category)
        {
            LSL_PRO_Roles rol = new LSL_PRO_Roles();
            rol.RoleId = CommonHelper.GetGuid;
            rol.FullName = name;
            rol.Category = category;
            rol.Enabled = 1;
            rol.CreateDate = DateTime.Now;
            bll.Add(rol, ref Rm);
            return Rm;
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoleInfo()
        {
            string id = Request["id"];
            bll.GetRoleByID(id, ref Rm);
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