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
            }
            else if (form["P2_change"] != null)
            {
                ParametriBoiler.P2 = Convert.ToInt32(form["P2_change"]);
            }
            else if(form["S5_Button"] != null)
            {
                if (ParametriBoiler.SystemEnable == true && ParametriBoiler.PompaG1 == false && ParametriBoiler.ValvaK1 == false)
                    ParametriBoiler.PompaG1 = true;
            }
            else if (form["S0_Button"] != null)
            {
                if (ParametriBoiler.SystemEnable == true && ParametriBoiler.PompaG1 == false && ParametriBoiler.ValvaK1 == false)
                {
                    ParametriBoiler.PompaG1 = true;
                    ParametriBoiler.ValvaK1 = true;
                }
            }
            else if(form["Refresh"] != null)
            {
                return Json(new { Message = Convert.ToByte(ParametriBoiler.NivelCurent*51/ParametriBoiler.Capacitate), JsonRequestBehavior.AllowGet });
            }
            return Json( new { Message = "success", JsonRequestBehavior.AllowGet } );
        }
    }
}