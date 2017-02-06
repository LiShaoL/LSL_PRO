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
    public class UserController : Controller
    {
        UserIBLL bll = new UserBLL();
        ResultModel Rm = new ResultModel();
        SessionUser su = RequestSession.GetSessionUser();
        [Authentication]
        public ActionResult User()
        {
            return View();
        }
        /// <summary>
        /// 添加用户界面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserAdd()
        {
            return View();
        }
        /// <summary>
        /// 分页获取列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserGetList()
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
            IList list = bll.GetList(sql, paramList.ToArray(), page, rows, sidx, sord, ref count);
            var jsonData = JqGridModel.GridData(page, rows, count, list);
            return Json(jsonData,JsonRequestBehavior.AllowGet);
        }
        //获取按钮
        [HttpPost]
        public ActionResult GetBtn()
        {
            string mid=Request["mid"];
            IList list = Common.GetBtnByRole(su.UserId,mid); 
            return Json(list);     
        }
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveInfo() 
        {
            string username = Request["name"];
            string pass = Request["pwd"];
            if (GetUser(username))
            {
                Rm = Update(username, pass);
            }
            else 
            {              
                Rm = Insert(username, pass);
            }          
            return Json(Rm);          
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Update(string username, string pass)
        {
            LSL_PRO_User user = new LSL_PRO_User();
            user.Account = username;
            user.Password = Md5Helper.MakeMd5(pass);
            bll.UpdateUser(user, ref Rm);
            return Rm;        
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private ResultModel Insert(string username, string pass)
        {
            LSL_PRO_User user = new LSL_PRO_User();
            user.UserId = CommonHelper.GetGuid;
            user.Account = username;
            user.Password = Md5Helper.MakeMd5(pass);
            bll.AddUser(user, ref Rm);
            return Rm;          
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool GetUser(string username)
        {
            bll.GetUserByAccount(username, ref Rm);
            if(Rm.code.Equals("1"))
            {
                return true;
            }else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserInfo()
        {
            string id = Request["id"];
            bll.GetUserByID(id, ref Rm);
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