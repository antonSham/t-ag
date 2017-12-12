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

        [HttpGet]
        public ActionResult Add()
        {
            User user = (User)Session["User"];

            if (user == null || user.role == "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            return View();
        }

        [HttpPost]
        public ActionResult Add(string country, string type, int price, string description, int sale, int year, int month, int day, int hour, int minute)
        {
            User user = (User)Session["User"];

            if (user == null || user.role == "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            Tour tour = new Tour();
            tour.country = country;
            tour.type = type;
            tour.price = price;
            tour.description = description;
            tour.sale = sale;
            tour.saleDate = new DateTime(year, month, day, hour, minute, 0);
            tour.feedbacks = new List<string>();
            TourDAO.addTour(tour);

            return RedirectToAction("Index", "Tour");
        }

        [HttpGet]
        public ActionResult Update(int? tourId)
        {
            User user = (User)Session["User"];

            if (user == null || user.role == "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            if (tourId == null)
            {
                return RedirectToAction("Index", "Tour");
            }

            ViewBag.tour = TourDAO.getTourById((int)tourId); ;

            return View();
        }

        [HttpPost]
        public ActionResult Update(int? tourId, string country, string type, int price, string description, int sale, int year, int month, int day, int hour, int minute)
        {
            User user = (User)Session["User"];

            if (user == null || user.role == "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;


            if (tourId == null)
            {
                return RedirectToAction("Index", "Tour");
            }

            Tour tour = TourDAO.getTourById((int)tourId);
            tour.country = country;
            tour.type = type;
            tour.price = price;
            tour.description = description;
            tour.sale = sale;
            tour.saleDate = new DateTime(year, month, day, hour, minute, 0);
            TourDAO.updateTour(tour);

            return RedirectToAction("More", "Tour", new { tourId=tourId });
        }

        [HttpGet]
        public ActionResult Delete(int? tourId)
        {
            User user = (User)Session["User"];

            if (user == null || user.role == "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;


            if (tourId == null)
            {
                return RedirectToAction("Index", "Tour");
            }

            TourDAO.deleteTourById((int)tourId);

            return RedirectToAction("Index", "Tour");
        }
    }
}