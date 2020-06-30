using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zoo.Models;

namespace zoo.Controllers
{
    public class CareGiverBaseAttractionController : Controller
    {
        // GET: CareGiverBaseAttraction
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            List<string> ErrorMsg = new List<string>();
            ErrorMsg.Add(" ");
            IEnumerable<Animal> MyAnimals = ViewMyAnimals();
            var tuple = new Tuple<IEnumerable<Animal>,IEnumerable<string>>(MyAnimals, ErrorMsg);
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

        [HttpPost]
        public ActionResult ViewMyAnimalCurrentAttr(Animal model)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                
                
                if (model.animal_name == null)
                {
                    List<string> ErrorMsg = new List<string>();
                    ErrorMsg.Add("Choose an Animal!");
                    IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                    var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<string>>(MyAnimals, ErrorMsg);
                    return View("~/Views/CareGiverBaseAttraction/Index.cshtml", tuple);
                }
                else
                {
                    if (db.Animals.Where(x => x.animal_name == model.animal_name).Select(y => y.Attraction_ID).FirstOrDefault() == null)
                    {
                        List<string> ErrorMsg = new List<string>();
                        ErrorMsg.Add("This Animal is not in any attractions!");
                        IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                        var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<string>>(MyAnimals, ErrorMsg);
                        return View("~/Views/CareGiverBaseAttraction/Index.cshtml", tuple);
                    }
                    else
                    {
                        System.Guid AttrID = (System.Guid)db.Animals.Where(x => x.animal_name == model.animal_name).Select(y => y.Attraction_ID).FirstOrDefault();
                        IEnumerable<Attraction> AttrReport = getNames(db.Attractions.Where(x => x.Attraction_ID == AttrID && x.isActive == true).ToList());
                        var tuple = new Tuple<IEnumerable<Attraction>>(AttrReport);
                        RedirectToAction("Index", "CareGiverBaseAttraction");
                        return View("~/Views/CareGiverBaseAttraction/CurrentAttr.cshtml", tuple);
                    }
                }

            }
        }

        [HttpPost]
        public ActionResult AnimalAttrHistory(Animal model)
        {
            using (team4zooEntities db = new team4zooEntities())
            {                                
                if (model.animal_name == null)
                {
                    List<string> ErrorMsg = new List<string>();
                    ErrorMsg.Add("Choose an Animal!");
                    IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                    var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<string>>(MyAnimals, ErrorMsg);
                    return View("~/Views/CareGiverBaseAttraction/Index.cshtml", tuple);
                }
                else
                {
                    if (db.Animals.Where(x => x.animal_name == model.animal_name).Select(y => y.Attraction_ID).FirstOrDefault() == null)
                    {
                        List<string> ErrorMsg = new List<string>();
                        ErrorMsg.Add("This Animal is not in any attractions!");
                        IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                        var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<string>>(MyAnimals, ErrorMsg);
                        return View("~/Views/CareGiverBaseAttraction/Index.cshtml", tuple);
                    }
                    else
                    {
                        System.Guid AttrID = (System.Guid)db.Animals.Where(x => x.animal_name == model.animal_name).Select(y => y.Attraction_ID).FirstOrDefault();
                        IEnumerable<Attraction> AttrReport = getNames(db.Attractions.Where(x => x.Attraction_ID == AttrID).ToList());
                        var tuple = new Tuple<IEnumerable<Attraction>>(AttrReport);
                        RedirectToAction("Index", "CareGiverBaseAttraction");
                        return View("~/Views/CareGiverBaseAttraction/AttrHistory.cshtml", tuple);
                    }
                }

            }

        }

        public IEnumerable<Attraction> getNames(IEnumerable<Attraction> MyAttr){

            using (team4zooEntities db = new team4zooEntities())
            {
                foreach (var item in MyAttr)
                {
                    string Fname = db.Employees.Where(x => x.Employee_ID == item.manager_ID).Select(y => y.f_name).FirstOrDefault();
                    string LName = db.Employees.Where(x => x.Employee_ID == item.manager_ID).Select(y => y.l_name).FirstOrDefault();
                    item.manager_name = Fname + " " + LName;

                }
                return MyAttr;
            }
        }
    }
}