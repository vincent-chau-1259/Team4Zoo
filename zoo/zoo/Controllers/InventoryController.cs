using zoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace zoo.Controllers
{
    public class InventoryController : Controller
    {
        // GET: CaregiverSuperAttraction
        public ActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddItem(Inventory Model, DateTime? last, DateTime? re)
        {

             using (team4zooEntities db = new team4zooEntities())
            {
                DateTime date = default(DateTime);
                if (last.HasValue)
                    date = last.Value;               
                try
                {
                    Guid ID = System.Guid.NewGuid();
                    Guid dept = new Guid("56320bbb-5776-42cc-8fac-42e648ec8719");
                    Inventory Item = new Inventory();
                    Item.Item_ID = ID;
                    Item.item_name = Model.item_name;
                    Item.last_ordered = date;
                    Item.resupply_date = re;
                    Item.price = Model.price;
                    Item.ordered_quantity = Model.ordered_quantity;
                    Item.Department_ID = dept;
                    Item.wholesaleprice = Model.wholesaleprice;
                    db.Inventory.Add(Item);
                    db.SaveChanges();
                }
                catch(Exception)
                {
                    ViewBag.Message = "All Fields Except Resupply Date Are Required";
                    return View();
                }

            }

            ViewBag.Message = "Item Added";
            return View();
        }
    }
}
