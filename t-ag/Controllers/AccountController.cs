using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using t_ag.DAO;
using t_ag.Models;

namespace t_ag.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Index2()
        {
            return View();
        }

        // GET: Account
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                ViewBag.authorized = true;
                User user = (User)Session["User"];
                ViewBag.name = user.login;
                ViewBag.role = user.role;
            }
            else
            {
                ViewBag.authorized = false;
            }

            return View();
        } 
    }
}