using SahaflarPazari.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SahaflarPazari.Controllers
{
    public class DataTestController : Controller
    {
        // GET: DataTest
        public JsonResult Index()
        {
           MyRoleProvider roleProvider = new MyRoleProvider();
            string[] data = roleProvider.GetRolesForUser("user1");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}