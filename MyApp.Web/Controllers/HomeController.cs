using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyApp.Framework.Infrastructure;
using MyApp.Models.Admin.Roles;
using MyApp.ServiceLayer.Roles;

namespace MyApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //for test
            var a = IoC.Resolve<IRoleApplicationService>();
            var b = a.GetList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}