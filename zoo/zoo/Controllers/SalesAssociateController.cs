using zoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace zoo.Controllers
{
    public class SalesAssociateController : Controller
    {
        // GET: SalesAssociate
        public ActionResult Login()
        {
            return RedirectToAction("Index", "Login");
        }

        public ActionResult TicketSales()
        {

            return View();
        }
        public ActionResult AddMember()
        {

            return View();
        }

        public ActionResult Sales()
        {

            return View();
        }

        public ActionResult Inventory()
        {

            return View();
        }
        public ActionResult Contact()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Contact(Employee Model)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                var Phone = db.Employees.Where(x => x.f_name == Model.f_name && x.l_name == Model.l_name).Select(y => y.phone_num).FirstOrDefault();
                var Email = db.Employees.Where(x => x.f_name == Model.f_name && x.l_name == Model.l_name).Select(y => y.email).FirstOrDefault();
                String contactInfo = " Phone: " + Phone + " " + ", " +  " Email: " + Email;
                if (Phone != null && Email != null)
                    ViewBag.Message = contactInfo;
                else
                    ViewBag.Message = "No one by that name was found";
                return View();
            }
        }
        [HttpPost]
        public ActionResult Inventory(Inventory Model)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                var Price = db.Inventory.Where(x => x.item_name == Model.item_name).Select(y => y.price).FirstOrDefault();
                var InStock = db.Inventory.Where(x => x.item_name == Model.item_name).Select(y => y.ordered_quantity).FirstOrDefault();
                var ItemName = db.Inventory.Where(x => x.item_name == Model.item_name).Select(y => y.item_name).FirstOrDefault();

                String ItemInfo = ItemName + " Price: " + Price + " " + ", " + " In Stock: " + InStock;
                if (ItemName != null)
                    ViewBag.Message = ItemInfo;
                else
                    ViewBag.Message = "No Item Found";
                return View();
            }
        }
         [HttpPost]
        public ActionResult Sales(Shop_Sale_Record Model, string ShopID, string itemName)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                var name = itemName;
                var number = "000-000-0000";
                var ID = db.Inventory.Where(x => x.item_name == itemName).Select(y => y.Item_ID).FirstOrDefault();
                var check = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.Department_ID).FirstOrDefault();
                var Member = db.Customers.Where(x => x.phone_number == number).Select(y => y.Customer_ID).FirstOrDefault();
                var Price = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.price).FirstOrDefault();
                var InStock = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.ordered_quantity).FirstOrDefault();
                var ItemName = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.item_name).FirstOrDefault();
                var amountPurchased = Model.quantity;
                var cost = Price * Model.quantity;
                var amountLeft = InStock;
                Guid dept = new Guid("7203f98e-e4dc-4edd-b221-98b5855fd4e3");
                Guid SID = new Guid(ShopID);
                if(check == dept)
                {
                    ViewBag.Message = "Transaction Failed: Shops Cannot Sell Tickets";
                    return View();
                }
                if(ItemName == null)
                {
                    ViewBag.Message = "No Item Found";
                    return View();
                }
                if (Model.refund_flag == false)
                {
                    amountLeft = InStock - Model.quantity;
                }
                else
                {
                    amountLeft = InStock + Model.quantity;
                }
                Model.Item_ID = ID;
                Model.Shop_ID = SID;
                if (number == "")
                { Guid id = new Guid("7117d230-9bb6-4c3b-8d38-2a9b9e8092b6");
                    Member = id;
                } 
                if (InStock >= amountPurchased)
                {
                    Shop_Sale_Record NewEntry = new Shop_Sale_Record();
                    NewEntry.Sale_ID = System.Guid.NewGuid();
                    NewEntry.Shop_ID = Model.Shop_ID;
                    NewEntry.Item_ID = Model.Item_ID;
                    NewEntry.quantity = Model.quantity;
                    NewEntry.Customer_ID = Member;
                    NewEntry.refund_flag = Model.refund_flag;
                    NewEntry.date = DateTime.Today;
                    db.Shop_Sale_Record.Add(NewEntry);
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("update zoo.Inventory set ordered_quantity = '" + amountLeft + "' where Item_Id = '" + Model.Item_ID + "'");

                    if (Model.refund_flag == false)
                    ViewBag.Message = "Transaction Complete: " + Model.quantity + " " + ItemName + " costs $" + cost;
                    else
                        ViewBag.Message = "Succsefully Refunded: " + Model.quantity + " " + ItemName + " for $" + cost;
                }
                else
                    ViewBag.Message = "Transaction Failed: We only have " + InStock + " " + ItemName + " in stock."; 
                return View();
            }
        }
        [HttpPost]
        public ActionResult TicketSales(Shop_Sale_Record Model, BoxOffice_Records Model2, string itemName, string phoneNum)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                var name = itemName;
                var number = phoneNum;
                var ID = db.Inventory.Where(x => x.item_name == itemName).Select(y => y.Item_ID).FirstOrDefault();
                var check = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.Department_ID).FirstOrDefault();
                var Member = db.Customers.Where(x => x.phone_number == number).Select(y => y.Customer_ID).FirstOrDefault();
                var InStock = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.ordered_quantity).FirstOrDefault();
                var Price = db.Inventory.Where(x => x.Item_ID == ID).Select(y => y.price).FirstOrDefault();
                var cost = Model2.quantity * Price;
                var amountLeft = InStock - Model2.quantity;
                decimal discount = 0;
                var totalCost = cost;
                Guid NM = new Guid("00000000-0000-0000-0000-000000000000");
                var nonMember = NM;
                Guid dept = new Guid("7203f98e-e4dc-4edd-b221-98b5855fd4e3");
                if(check != dept)
                {
                    ViewBag.Message = "Transaction Failed: Not a Ticket";
                    return View();
                }
                if ( Member != nonMember)
                {
                    discount = cost / 10;
                    totalCost = cost - discount;
                    BoxOffice_Records NewEntry = new BoxOffice_Records();
                    NewEntry.Sale_ID = System.Guid.NewGuid();
                    NewEntry.Department_ID = dept;
                    NewEntry.membership_discount = discount;
                    NewEntry.quantity = Model2.quantity;
                    NewEntry.Customer_ID = Member;
                    NewEntry.tot_amt_paid_after_discounts = totalCost;
                    NewEntry.date = DateTime.Today;
                    db.BoxOffice_Records.Add(NewEntry);
                    db.SaveChanges();
                    ViewBag.Message = "Transaction Completed: Cost: " + cost + " Member Discount: " + discount + " Total Cost: " + totalCost;
                    db.Database.ExecuteSqlCommand("update zoo.Inventory set ordered_quantity = '" + amountLeft + "' where Item_Id = '" + ID + "'");

                }
                else
                {
                    Guid id = new Guid("7117d230-9bb6-4c3b-8d38-2a9b9e8092b6");
                    Member = id;
                    BoxOffice_Records NewEntry = new BoxOffice_Records();
                    NewEntry.Sale_ID = System.Guid.NewGuid();
                    NewEntry.Department_ID = dept;
                    NewEntry.membership_discount = discount;
                    NewEntry.quantity = Model2.quantity;
                    NewEntry.Customer_ID = Member;
                    NewEntry.tot_amt_paid_after_discounts = totalCost;
                    NewEntry.date = DateTime.Today;
                    db.BoxOffice_Records.Add(NewEntry);
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("update zoo.Inventory set ordered_quantity = '" + amountLeft + "' where Item_Id = '" + ID + "'");
                    ViewBag.Message = "Transaction Completed: cost: "+ cost;
                }
               
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddMember(Customer Model)
        {
             using (team4zooEntities db = new team4zooEntities())
            {
                if (Model.f_name == null || Model.l_name == null || Model.phone_number == null)
                {
                    ViewBag.Message = "Must Enter All Fields";
                    return View();
                }
                else
                {
                    Customer NewEntry = new Customer();
                    NewEntry.Customer_ID = System.Guid.NewGuid();
                    NewEntry.f_name = Model.f_name;
                    NewEntry.l_name = Model.l_name;
                    NewEntry.phone_number = Model.phone_number;
                    NewEntry.membership = true;
                    db.Customers.Add(NewEntry);
                    db.SaveChanges();
                    ViewBag.Message = "Member Added";
                    return View();
                }
            }
        }
    }
}
