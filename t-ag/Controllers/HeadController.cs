using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using t_ag.Models;
using t_ag.DAO;

namespace t_ag.Controllers
{
    public class HeadController : Controller
    {
        // GET: Head
        [HttpGet]
        public ActionResult Index()
        {
            User user = (User)Session["User"];
            if (user == null || user.role != "head")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.user = user;

            ViewBag.allUsers = UserDAO.getAllUsers();

            return View();
        }

        [HttpPost]
        public ActionResult Index(string userId, string newRole)
        {
            User user = (User)Session["User"];
            if (user == null || user.role != "head")
            {
                return RedirectToAction("Index", "Home");
            }

            UserDAO.updateRole(Int32.Parse(userId), newRole);

            return RedirectToAction("Index", "Head");
        }
    }
}