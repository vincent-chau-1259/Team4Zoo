using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using zoo.Models;

namespace zoo.Controllers
{
    public class SalesManagerController : Controller
    {
        // GET: SalesManager
        public ActionResult Index()
        {
            IEnumerable<Employee> EmployeeList = ViewMyEmployees();
            List<MyEmployee> Employees = new List<MyEmployee>();
            foreach (var person in EmployeeList)
            {
                Employees.Add(new MyEmployee(person.f_name, person.l_name, person.email, person.phone_num));
            }
            return View(Employees);
        }

        public ActionResult Item()
        {
            using (team4zooEntities DB = new team4zooEntities())
            {
                List<Inventory> itemlist = DB.Inventory.ToList();

                return View(itemlist);
            }
        }

        public IEnumerable<Employee> ViewMyEmployees()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid MyEmployee_ID = (System.Guid)Session["Employee_ID"];
                return db.Employees.ToList().Where(x => (x.Supervisor_ID == MyEmployee_ID));
            }
        }
        public ActionResult Login()
        {
            return RedirectToAction("Index", "Login");
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
                String contactInfo = " Phone: " + Phone + " " + ", " + " Email: " + Email;
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

        public ActionResult Shop()
        {
            team4zooEntities DB = new team4zooEntities();

            List<Shop> shoplist = DB.Shops.ToList();
            return View(shoplist);
        }

        public ActionResult ViewInventory()
        {
            team4zooEntities db = new team4zooEntities();
            
            List<Inventory> inventorylist = db.Inventory.ToList();
            return View(inventorylist);
        }

        [HttpGet]
        public ActionResult editItem(string id)
        {
            using (team4zooEntities DB = new team4zooEntities())
            {
                Guid ID = new Guid(id);
                Inventory item = DB.Inventory.Single(Inventory => Inventory.Item_ID == ID);

                return View(item);
            }
        }

        //This creates a SQL ready item object and adds to database
        public void SaveItem(Inventory item)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=den1.mssql8.gear.host;Database=team4zoo;Uid=team4zoo;Pwd=Ji627i1J-x5?"; //Connection String with login info.

                //Call Query
                SqlCommand cmd = new SqlCommand("SaveItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //Set Parameters
                SqlParameter paramIName = new SqlParameter();//ItemName
                paramIName.ParameterName = "@item_name";
                paramIName.Value = item.item_name;
                cmd.Parameters.Add(paramIName);

                SqlParameter paramID = new SqlParameter();//ItemID
                paramID.ParameterName = "@Item_ID";
                paramID.Value = item.Item_ID;
                cmd.Parameters.Add(paramID);

                SqlParameter paramPrice = new SqlParameter();//Price
                paramPrice.ParameterName = "@price";
                paramPrice.Value = item.price;
                cmd.Parameters.Add(paramPrice);

                SqlParameter paramQuantity = new SqlParameter();//Quantity
                paramQuantity.ParameterName = "@ordered_quantity";
                paramQuantity.Value = item.ordered_quantity;
                cmd.Parameters.Add(paramQuantity);

                SqlParameter paramWPrice = new SqlParameter(); //Wholesale Price
                paramWPrice.ParameterName = "@wholesaleprice";
                paramWPrice.Value = item.wholesaleprice;
                cmd.Parameters.Add(paramWPrice);


                conn.Open(); //Opens connection
                cmd.ExecuteNonQuery(); //Add to table
            }
        }

        [HttpPost]
        public ActionResult editItem(Inventory item)
        {

            if (ModelState.IsValid && item.item_name.Length > 0 && item.price >= 0 && item.ordered_quantity >= 0 && item.wholesaleprice >= 0)
            {
                SalesManagerController salesmController = new SalesManagerController();

                salesmController.SaveItem(item);

                ViewBag.Message = "Edit Successful";

                return RedirectToAction("ViewInventory");
            }
            else
            {
                ViewBag.Message = "Invalid Input";
                return View();
            }
        }

        //DeleteItem Method
        public void DeleteInventory(Guid id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=den1.mssql8.gear.host;Database=team4zoo;Uid=team4zoo;Pwd=Ji627i1J-x5?"; //Connection String with login info.

                //Call Query
                SqlCommand cmd = new SqlCommand("DeleteInventory", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //Set Parameters
                SqlParameter paramID = new SqlParameter();//ItemName
                paramID.ParameterName = "@Item_ID";
                paramID.Value = id;
                cmd.Parameters.Add(paramID);

                conn.Open(); //Opens connection
                cmd.ExecuteNonQuery(); //Add to table
            }
        }

        [HttpPost]
        public ActionResult DeleteItem(string id)
        {
            Guid guid = new Guid(id);
            DeleteInventory(guid);
            return RedirectToAction("ViewInventory");
        }


    }
}