using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PS2IMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Stopwatch stopwatch;
            return View();
        }
    }
}