using zoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace zoo.Controllers
{
    public class CaregiverSuperAttractionController : Controller
    {
        // GET: CaregiverSuperAttraction
        public ActionResult AddAttraction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAttraction(Attraction Model, DateTime from, DateTime to, TimeSpan Tfrom, TimeSpan Tto, decimal cost, int amount)
        {
            
            using (team4zooEntities db = new team4zooEntities())
            {
                Guid ID = System.Guid.NewGuid();
                var key = ID;
                Attraction NewEntry = new Attraction();
                NewEntry.manager_ID = (System.Guid)Session["Employee_ID"];
                NewEntry.attraction_name = Model.attraction_name;
                NewEntry.start_date = from;
                NewEntry.end_date = to;
                NewEntry.start_time = Tfrom;
                NewEntry.end_time = Tto;
                NewEntry.isActive = true;
                NewEntry.Attraction_ID = key; 
                db.Attractions.Add(NewEntry);
                db.SaveChanges();
                ViewBag.Message = "Attraction Added";

                Guid dept = new Guid("7203f98e-e4dc-4edd-b221-98b5855fd4e3");
                Inventory Ticket = new Inventory();
                Ticket.Item_ID = key;
                Ticket.item_name = Model.attraction_name;
                Ticket.last_ordered = from;
                Ticket.price = cost;
                Ticket.ordered_quantity = amount;
                Ticket.Department_ID = dept;
                db.Inventory.Add(Ticket);
                db.SaveChanges();
            }
                return View();
        }
        public ActionResult ViewAttractions()
        {
            team4zooEntities db = new team4zooEntities();

            List<Attraction> attractionlist = db.Attractions.ToList();
            List<Attraction> SortedAttractions = attractionlist.OrderBy(x => x.start_date).ToList();
            return View(SortedAttractions);
        }
    }
}