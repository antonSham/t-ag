using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using t_ag.Models;
using t_ag.DAO;

namespace t_ag.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult Index()
        {
            User user = (User)Session["User"];

            if (user == null || user.role == "customer")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.user = user;

            ViewBag.countryStatistics = StatisticsDAO.getCountryStatistics();
            ViewBag.typeStatistics = StatisticsDAO.getTypeStatistics();

            return View();
        }
    }
}