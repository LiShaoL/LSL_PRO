using LSL_PRO.Utilities;
using LSL_PRO_BLL;
using LSL_PRO_IBLL;
using LSL_PRO_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSL_PRO_WEB.Controllers
{
    public class IndexController : Controller
    {
        IndexIBLL bll = new IndexBLL();
        SessionUser su = RequestSession.GetSessionUser();

        [Authentication]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndexP()
        {

            IList list = bll.GetList(su.UserId);
            return Json(list);          
        }
	}
}