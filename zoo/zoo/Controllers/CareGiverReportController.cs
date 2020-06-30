using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zoo.Models;

namespace zoo.Controllers
{
    public class CareGiverReportController : Controller
    {
        // GET: CareGiverReport
        public ActionResult Index ()
        {
            List<string> ErrorMsg = new List<string>();
            ErrorMsg.Add(" ");
            IEnumerable<Animal> MyAnimals = ViewMyAnimals();
            var tuple = new Tuple<IEnumerable<Animal>,IEnumerable<string> >(MyAnimals, ErrorMsg);
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
        public ActionResult ViewFoodReport(Animal model, DateTime? from, DateTime? to)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
               System.Guid Animal_ID = db.Animals.Where(x => x.animal_name == model.animal_name).Select(y => y.Animal_ID).FirstOrDefault();
               if(!from.HasValue || !to.HasValue || model.animal_name == null)
                {
                    List<string> ErrorMsg = new List<string>();
                    ErrorMsg.Add("Fill All the Fields!");
                    IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                    var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<string>>(MyAnimals, ErrorMsg);
                    return View("~/Views/CareGiverReport/Index.cshtml", tuple);
                }
                else
                {
               //     IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                    IEnumerable<Animal_Feed_Care> FeedReport = db.Animal_Feed_Care.Where(x => x.Animal_ID == Animal_ID && (x.date >= from || x.date <= to)).ToList();
                    var tuple = new Tuple<IEnumerable<Animal_Feed_Care>>(FeedReport);
                    return View("~/Views/CareGiverReport/Report.cshtml",tuple);
                }

            }
            
        }

        [HttpPost]
        public ActionResult ViewMedicationReport(Animal model, DateTime? from, DateTime? to)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid Animal_ID = db.Animals.Where(x => x.animal_name == model.animal_name).Select(y => y.Animal_ID).FirstOrDefault();
                if (!from.HasValue || !to.HasValue || model.animal_name == null)
                {
                    List<string> ErrorMsg = new List<string>();
                    ErrorMsg.Add("Fill All the Fields!");
                    IEnumerable<Animal> MyAnimals = ViewMyAnimals();
                    var tuple = new Tuple<IEnumerable<Animal>, IEnumerable<string>>(MyAnimals, ErrorMsg);
                    return View("~/Views/CareGiverReport/Index.cshtml", tuple);
                }
                else
                {
                    IEnumerable<Animal_Medication_Care> MedicationReport = db.Animal_Medication_Care.Where(x => x.Animal_ID == Animal_ID && (x.date >= from || x.date <= to)).ToList();
                    var tuple = new Tuple<IEnumerable<Animal_Medication_Care>>(MedicationReport);
                    return View("~/Views/CareGiverReport/MedicationReport.cshtml", tuple);
                }

            }

        }
    }
}