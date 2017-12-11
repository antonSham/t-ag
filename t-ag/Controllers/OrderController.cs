using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using t_ag.Models;
using t_ag.DAO;

namespace t_ag.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;
            if (user.role == "customer")
            {
                ViewBag.mine = true;
                ViewBag.allOrders = OrderDAO.getAllOrders().Where(el => el.customer.login == user.login).ToList();
            }
            else
            {
                ViewBag.mine = false;
                ViewBag.allOrders = OrderDAO.getAllOrders();
            }
            

            return View();
        }

        public ActionResult More(int? orderId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            if (orderId == null)
            {
                return RedirectToAction("Index", "Order");
            }
            try
            {
                ViewBag.order = OrderDAO.getOrderById((int)orderId);
            }
            catch (DOAException)
            {
                return RedirectToAction("Index", "Order");
            }


            return View();
        }
    }
}