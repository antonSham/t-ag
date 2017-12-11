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

        [HttpGet]
        public ActionResult Make(int? tourId, int? orderId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            if (orderId == null)
            {
                if (tourId == null)
                {
                    return RedirectToAction("Index", "Tour");
                }

                Tour tour;
                try
                {
                    tour = TourDAO.getTourById((int)tourId);
                }
                catch (DOAException)
                {
                    return RedirectToAction("Index", "Tour");
                }
                Order order = new Order();
                order.tour = tour;
                order.customer = user;
                order.participants = new List<Participant>();
                order.id = OrderDAO.addOrder(order);
                ViewBag.order = order;
            } else
            {
                ViewBag.order = OrderDAO.getOrderById((int)orderId);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Make(string fullName, int age, string passport, int orderId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            Participant participant = new Participant();
            participant.fullName = fullName;
            participant.age = Convert.ToInt32(age);
            participant.passport = passport;

            participant.id = ParticipantDAO.addParticipant(participant);

            OrderDAO.addParticipant(orderId, participant.id);

            return RedirectToAction("Make", "Order", new { orderId=orderId } );
        }

        [HttpPost]
        public ActionResult Commit(int orderId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            OrderDAO.commitOrder(orderId);

            return RedirectToAction("Index", "Order");
        }

        [HttpGet]
        public ActionResult Cancel(int orderId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            OrderDAO.cancelOrder(orderId);

            return RedirectToAction("Index", "Order");
        }

        [HttpGet]
        public ActionResult CommitAmmount(int orderId)
        {
            User user = (User)Session["User"];

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = user;

            OrderDAO.commitOrderAmount(orderId);

            return RedirectToAction("More", "Order", new { orderId = orderId });
        }
    }
}