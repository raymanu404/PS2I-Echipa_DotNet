using PS2IMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PS2IMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if(form["button_enable"] != null)
            {
                if(form["button_enable"] == "false")
                {
                    ParametriBoiler.SystemEnable = false;
                }
                else if(form["button_enable"] == "true")
                {
                    ParametriBoiler.SystemEnable = true;
                }
            }
            else if(form["P1_change"] != null)
            {
                ParametriBoiler.P1 = Convert.ToInt32(form["P1_change"]);
                //Debug.WriteLine(ParametriBoiler.P1);
            }
            else if (form["P2_change"].ToString() != null)
            {
                ParametriBoiler.P2 = Convert.ToInt32(form["P2_change"]);
                //Debug.WriteLine(ParametriBoiler.P2);
            }
            return Json( new { Message = "success", JsonRequestBehavior.AllowGet } );
        }
    }
}