using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using zoo.Models;

namespace zoo.Controllers
{
    public class CaregiverSuperController : Controller
    {

        // GET: CaregiverSuperHome
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

        public IEnumerable<Employee> ViewMyEmployees()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid MyEmployee_ID = (System.Guid)Session["Employee_ID"];
                return db.Employees.ToList().Where(x => (x.Supervisor_ID == MyEmployee_ID));
            }
        }
        public ActionResult MyAnimals()
        {
            using (team4zooEntities DB = new team4zooEntities())
            {
                List<Animal> AnimalList = DB.Animals.ToList();

                return View(AnimalList);
            }
        }
        public IEnumerable<Animal> ViewMyAnimals()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid Employee_ID = (System.Guid)Session["Employee_ID"];
                return db.Animals.ToList();
            }
        }
        [HttpGet] //HTTP GET to ensure addDept is reached
        public ActionResult AddAnim()
        {

            return View();
        }

        //This creates a SQL ready department object and adds to database
        public void AddAnimal(Animal animal)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=den1.mssql8.gear.host;Database=team4zoo;Uid=team4zoo;Pwd=Ji627i1J-x5?"; //Connection String with login info.

                //Call Query
                SqlCommand cmd = new SqlCommand("AddAnimal", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //Set Parameters
                SqlParameter paramName = new SqlParameter(); //Name
                paramName.ParameterName = "@animal_name";
                paramName.Value = animal.animal_name;
                cmd.Parameters.Add(paramName);

                SqlParameter paramDOB = new SqlParameter(); //DOB
                paramDOB.ParameterName = "@dob";
                paramDOB.Value = animal.dob;
                cmd.Parameters.Add(paramDOB);

                SqlParameter paramW = new SqlParameter(); //Weight
                paramW.ParameterName = "@weight";
                paramW.Value = animal.weight;
                cmd.Parameters.Add(paramW);

                SqlParameter paramSex = new SqlParameter(); //Sex
                paramSex.ParameterName = "@sex";
                paramSex.Value = animal.sex;
                cmd.Parameters.Add(paramSex);

    
                SqlParameter paramExhibit = new SqlParameter(); //Exhibit
                paramExhibit.ParameterName = "@Exhibit_ID";
                paramExhibit.Value = animal.Exhibit_ID;
                cmd.Parameters.Add(paramExhibit);


                SqlParameter paramAsgn1 = new SqlParameter(); //Assignee 1
                paramAsgn1.ParameterName = "@Assignee1_ID";
                paramAsgn1.Value = animal.Assignee1_ID;
                cmd.Parameters.Add(paramAsgn1);


                SqlParameter paramAsgn2 = new SqlParameter(); //Assignee 2
                paramAsgn2.ParameterName = "@Assignee2_ID";
                paramAsgn2.Value = animal.Assignee2_ID;
                cmd.Parameters.Add(paramAsgn2);


                SqlParameter paramAsgn3 = new SqlParameter(); //Assignee 3
                paramAsgn3.ParameterName = "@Assignee3_ID";
                paramAsgn3.Value = animal.Assignee3_ID;
                cmd.Parameters.Add(paramAsgn3);


                conn.Open(); //Opens connection
                cmd.ExecuteNonQuery(); //Add to table
            }
        }

        [HttpPost]
        public ActionResult AddAnim(FormCollection formCollection)
        {

            if (ModelState.IsValid && formCollection["animal_name"].Length > 0)
            {
                //Get Data From Form
                Animal animal = new Animal();
                animal.animal_name = formCollection["animal_name"];
                animal.family = Convert.ToInt16(formCollection["family"]);
                animal.dob = Convert.ToDateTime(formCollection["dob"]);
                animal.weight = Convert.ToDecimal(formCollection["weight"]);
                animal.sex = formCollection["sex"];
                animal.isActive = true;

                Guid ID1 = new Guid(formCollection["Assignee1_ID"]);
                Guid ID2 = new Guid(formCollection["Assignee2_ID"]);
                Guid ID3 = new Guid(formCollection["Assignee3_ID"]);

                animal.Assignee1_ID = ID1;
                animal.Assignee1_ID = ID2;
                animal.Assignee1_ID = ID3;

                animal.assignee1_name = converToName(ID1);
                animal.assignee2_name = converToName(ID2);
                animal.assignee3_name = converToName(ID3);


                CaregiverSuperController insertController = new CaregiverSuperController();

                insertController.AddAnimal(animal);

                return RedirectToAction("Animal");
            }
            else
            {
                ViewBag.Message = "Invalid Input";
                return View();
            }
        }
        public string converToName(Guid ID)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                bool exists = true;
                string fullname = " ";
                if (ID == null)
                {
                    exists = false;
                }

                List<Employee> AllEmployees = db.Employees.ToList();

                foreach (var person in AllEmployees)
                {
                    if (exists && person.Employee_ID == ID)
                    {
                        fullname = person.f_name + " " + person.mid_init + " " + person.l_name;
                    }
                    else
                    {
                        fullname = "none";
                    }
                }
                return fullname;
            }
        }
    }
}
