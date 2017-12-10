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
        public string Index(RequestContext requestContext)
        {
            List<User> L = UserDAO.getAllUsers();

            String str = "";

            L.ForEach(el => str += el.toString() + "<br />");

            return UserDAO.getUserById(1).toString() + "<br />Users <br />" + str;
        } 
    }
}