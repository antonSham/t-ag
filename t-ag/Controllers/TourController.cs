using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using t_ag.Models;
using t_ag.DAO;

namespace t_ag.Controllers
{
    public class TourController : Controller
    {
        // GET: Tour
        public ActionResult Index()
        {   
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;
            ViewBag.allTours = TourDAO.getAllTours();

            return View();
        }

        public ActionResult More(int? tourId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            if (tourId == null)
            {
                return RedirectToAction("Index", "Tour");
            }
            try
            {
                ViewBag.tour = TourDAO.getTourById((int)tourId);
            }
            catch (DOAException)
            {
                return RedirectToAction("Index", "Tour");
            }
            

            return View();
        }

        public ActionResult Feedback(int? orderId, string feedback)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            Order order;
            if (orderId == null)
            {
                return RedirectToAction("Index", "Order");
            }
            try
            {
                order = OrderDAO.getOrderById((int)orderId);
            }
            catch (DOAException)
            {
                return RedirectToAction("Index", "Order");
            }

            if (order.customer.id != user.id)
            {
                return RedirectToAction("Index", "Order");
            }

            TourDAO.addFeedback(order.tour.id, feedback);

            return RedirectToAction("More", "Order", new { orderId=order.id });
        }
    }
}