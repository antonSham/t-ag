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
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.user = (User)Session["User"];

            return View();
        }

        //GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            if ((User)Session["User"] != null)
            {
                return RedirectToAction("Index", "Account");
            }
            if (Session["Account_Login_Failed"] != null)
            {
                ViewBag.loging_message = "Try again";
                Session["Account_Login_Failed"] = null;
            }
            
            return View();
        }

        [HttpPost]
        public RedirectResult Login(string login, string password)
        {
            List<User> users = UserDAO.getAllUsers().Where(el => el.login == login && el.password == password).ToList();
            if (users.Count > 0)
            {
                Session["User"] = users[0];
            }
            else
            {
                Session["Account_Login_Failed"] = true;
                return Redirect("/Account/Login");
            }

            return Redirect("/");
        }

        [HttpGet]
        public RedirectResult Logout()
        {
            Session["User"] = null;

            return Redirect("/");
        }

       [HttpGet]
        public ActionResult Register()
        {
            if ((User)Session["User"] != null)
            {
                return RedirectToAction("Index", "Account");
            }
            if (Session["Account_Register_Busy"] != null)
            {
                ViewBag.error_message = "Login alrady exists";
                Session["Account_Register_Busy"] = null;
            } else if (Session["Account_Register_NotSame"] != null)
            {
                ViewBag.error_message = "Passwords are not same";
                Session["Account_Register_NotSame"] = null;
            }
            return View();
        }

        [HttpPost]
        public RedirectResult Register(string login, string password1, string password2)
        {
            List<User> users = UserDAO.getAllUsers().Where(el => el.login == login).ToList();
            if (users.Count > 0)
            {
                Session["Account_Register_Busy"] = true;
                return Redirect("/Account/Register");
            }

            if (password1 != password2)
            {
                Session["Account_Register_NotSame"] = true;
                return Redirect("/Account/Register");
            }

            User user = new User();
            user.login = login;
            user.password = password1;
            user.role = "customer";
            UserDAO.addUser(user);

            users = UserDAO.getAllUsers().Where(el => el.login == login && el.password == password1).ToList();

            Session["User"] = users[0];

            return Redirect("/");
        }
    }
}