using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zoo.Models;

namespace zoo.Controllers
{
    public class FeedCareController : Controller
    {
       
        // GET: FeedCare
        public ActionResult Index()
        {
            IEnumerable<Animal> MyAnimals = ViewMyAnimals();
            IEnumerable<Food_Supply> Food = GetFoodTypes();
            List<string> ErrorMsg = new List<string>();
            ErrorMsg.Add(" ");
            var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<Food_Supply>,IEnumerable<string>>(MyAnimals, Food, ErrorMsg);
            return View(tuple);
        }

        public IEnumerable<Animal> ViewMyAnimals()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid Employee_ID = (System.Guid)Session["Employee_ID"];
                return db.Animals.ToList().Where(x => (x.Assignee1_ID == Employee_ID || x.Assignee2_ID == Employee_ID || x.Assignee3_ID == Employee_ID));
            }
        }

        public IEnumerable<Food_Supply> GetFoodTypes()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                //yield return db.Food_Supply.ToList().Select(y => y.Food_type).FirstOrDefault();
                return db.Database.SqlQuery<Food_Supply>("select * from zoo.Food_Supply").ToList();
            }

        }

        [HttpPost]
        public ActionResult FoodEntery(Animal animalObj,Food_Supply foodObj)
        {
            using (team4zooEntities db = new team4zooEntities()) {
                Animal_Feed_Care NewEntry = new Animal_Feed_Care();
                NewEntry.Animal_ID = (System.Guid) db.Animals.Where(x => x.animal_name == animalObj.animal_name).Select(y => y.Animal_ID).FirstOrDefault();
                NewEntry.food_type = foodObj.Food_type;
                NewEntry.quantity_fed = foodObj.quantity_fed;
                NewEntry.Employee_ID = (System.Guid)Session["Employee_ID"];
                NewEntry.date = DateTime.Today;
                NewEntry.time = DateTime.Now.Subtract(DateTime.Today);

               
                if (animalObj.animal_name == null || foodObj.Food_type == null || foodObj.quantity_fed.Equals(0.00) )
                {
                    IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                    IEnumerable<Food_Supply> Food = GetFoodTypes();
                    List<string> ErrorMsg = new List<string>();
                    ErrorMsg.Add("Fill All the Fields!");
                    var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<Food_Supply>, IEnumerable<string>>(MyAnimals, Food, ErrorMsg);
                    return View("~/Views/FeedCare/Index.cshtml", tuple);
                }
                else
                {
                    //add new food entry to database
                    db.Animal_Feed_Care.Add(NewEntry);
                    //save changes to database
                    db.SaveChanges();
                    return RedirectToAction("Index", "FeedCare");
                }
            }
        }

        [HttpPost]
        public ActionResult MedicationEntery(Animal Model)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
               
                try
                {
                    Animal_Medication_Care NewEntry = new Animal_Medication_Care();
                    NewEntry.Animal_ID = db.Animals.Where(x => x.animal_name == Model.animal_name).Select(y => y.Animal_ID).FirstOrDefault();
                    NewEntry.medication = Model.medication;
                    NewEntry.dose = Model.dose;
                    NewEntry.description = Model.description;
                    NewEntry.Employee_ID = (System.Guid)Session["Employee_ID"];
                    NewEntry.date = DateTime.Today;
                    NewEntry.time = DateTime.Now.Subtract(DateTime.Today);
                    NewEntry.vet = Model.vet;
                    if (Model.animal_name == null || Model.medication==null || Model.dose == null || Model.vet == null || Model.description == null)
                    {
                        IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                        IEnumerable<Food_Supply> Food = GetFoodTypes();
                        List<string> ErrorMsg = new List<string>();
                        ErrorMsg.Add("Fill All the Fields!");
                        var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<Food_Supply>, IEnumerable<string>>(MyAnimals, Food, ErrorMsg);
                        return View("~/Views/FeedCare/Index.cshtml", tuple);
                    }
                    else
                    {
                        //add new food entry to database
                        db.Animal_Medication_Care.Add(NewEntry);
                        //save changes to database
                        db.SaveChanges();
                        return RedirectToAction("Index", "FeedCare");
                    }

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }
    }
}