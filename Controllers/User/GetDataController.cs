using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDemo.Controllers.User
{
    public class GetDataController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            // 验证session是否有效
            return Json(Session["user"]);
        }


    }
}