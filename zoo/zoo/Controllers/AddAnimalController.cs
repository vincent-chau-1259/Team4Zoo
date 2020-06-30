using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zoo.Models;

namespace zoo.Controllers
{
    public class AddAnimalController : Controller
    {
        
        // GET: AddAnimal
        public ActionResult Index()
        {
            return View(modelMaker());
        }

        public AddAnimal modelMaker()
        {
            AddAnimal Obj = new AddAnimal();
            Obj.familyNames = GetFamilyNames();
            Obj.emplyeeUserNames = ViewMyEmployees();
            Obj.exhibitNames = GetExihibitNames();
            Obj.attrNames = GetAttrNames();
            List<char> sex = new List<char>();
            sex.Add('M'); sex.Add('F');
            Obj.sexs = sex;
            return Obj;
        }

        public IEnumerable<string> ViewMyEmployees()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid MyEmployee_ID = (System.Guid)Session["Employee_ID"];
                return db.Employees.ToList().Where(x => (x.Supervisor_ID == MyEmployee_ID)).Select(y => y.username);
            }
        }

        public IEnumerable<string> GetFamilyNames()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
               return db.Family_Name.ToList().Select(y => y.family_title);
            }
        }

        public IEnumerable<string> GetExihibitNames()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                return db.Exhibits.ToList().Select(y => y.exhibit_name);
            }
        }

        public IEnumerable<string> GetAttrNames()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                return db.Attractions.ToList().Select(y => y.attraction_name);
            }
        }

        public bool CheckExistingName(string Name)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                var User = db.Animals.Where(x => x.animal_name == Name).FirstOrDefault();
                //check if user exists in the database already
                if (User == null) //not in db
                {
                    return false;
                }
                else
                    return true; //return true if user is in db
            }

        }

        [HttpPost]
        public ActionResult AddAnimal(AddAnimal model, DateTime? dob)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                try
                {
                    Animal animal = new Animal();
                    animal.Animal_ID = Guid.NewGuid();
                    animal.animal_name = model.name;
                    animal.dob = dob;
                    animal.sex = model.sex;
                    animal.weight = model.weight;
                    animal.owner = model.owner;
                    animal.isActive = true;
                    animal.family = db.Family_Name.Where(x => x.family_title == model.familyN).Select(y => y.Family_ID).FirstOrDefault();
                    if (model.name == null || model.sex == null || model.weight.Equals(null)
                        || model.weight.Equals(0.00) || !dob.HasValue || model.owner == null || model.ExhibitN == null)
                    {
                        AddAnimal newModel = modelMaker();
                        newModel.ErrorMessage1 = "Please, Fill all required fields.";
                        return View("~/Views/AddAnimal/Index.cshtml", newModel);
                    }
                    else if (CheckExistingName(model.name))
                    {
                        AddAnimal newModel = modelMaker();
                        newModel.ErrorMessage2 = "This Name Already Exists.";
                        return View("~/Views/AddAnimal/Index.cshtml", newModel);
                    }
                    else
                    {
                        if (model.AttrN != null)
                        {
                            animal.Attraction_ID = db.Attractions.Where(x => x.attraction_name == model.AttrN).Select(y => y.Attraction_ID).FirstOrDefault();
                        }
                        animal.Exhibit_ID = db.Exhibits.Where(x => x.exhibit_name == model.ExhibitN).Select(y => y.Exhibit_ID).FirstOrDefault();
                        animal.Assignee1_ID = db.Employees.Where(x => x.username == model.Assignee1).Select(y => y.Employee_ID).FirstOrDefault();
                        animal.Assignee2_ID = db.Employees.Where(x => x.username == model.Assignee2).Select(y => y.Employee_ID).FirstOrDefault();
                        animal.Assignee3_ID = db.Employees.Where(x => x.username == model.Assginee3).Select(y => y.Employee_ID).FirstOrDefault();

                        db.Animals.Add(animal);
                        db.SaveChanges();
                        return RedirectToAction("Index", "AddAnimal");
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