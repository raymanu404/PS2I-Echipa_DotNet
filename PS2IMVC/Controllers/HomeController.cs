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
            ViewBag.PragB1 = Convert.ToInt32(ParametriBoiler.PragB1 * 54 / ParametriBoiler.Capacitate);
            ViewBag.PragB2 = Convert.ToInt32(ParametriBoiler.PragB2 * 54 / ParametriBoiler.Capacitate);
            ViewBag.PragB3 = Convert.ToInt32(ParametriBoiler.PragB3 * 54 / ParametriBoiler.Capacitate);
            ViewBag.PragB4 = Convert.ToInt32(ParametriBoiler.PragB4 * 54 / ParametriBoiler.Capacitate);
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
                bool stateS0 = false, stateS5 = false;
                if (ParametriBoiler.PompaG1 == true && ParametriBoiler.ValvaK1 == false)
                {
                    stateS0 = false;
                    stateS5 = true;
                }
                else if(ParametriBoiler.PompaG1 == true && ParametriBoiler.ValvaK1 == true)
                {
                    stateS0 = true;
                    stateS5 = false;
                }
                else if(ParametriBoiler.PompaG1 == false && ParametriBoiler.ValvaK1 == false)
                {
                    stateS0 = false;
                    stateS5 = false;
                }
                return Json(new { Nivel = Convert.ToByte(ParametriBoiler.NivelCurent*55/ParametriBoiler.Capacitate), PragB1 = Convert.ToInt32(ParametriBoiler.PragB1 * 54 / ParametriBoiler.Capacitate), PragB2 = Convert.ToInt32(ParametriBoiler.PragB2 * 54 / ParametriBoiler.Capacitate), PragB3 = Convert.ToInt32(ParametriBoiler.PragB3 * 54 / ParametriBoiler.Capacitate), PragB4 = Convert.ToInt32(ParametriBoiler.PragB4 * 54 / ParametriBoiler.Capacitate), StateS0 = stateS0, StateS5 = stateS5});
            }
            return Json( new { Message = "success", JsonRequestBehavior.AllowGet } );
        }
    }
}