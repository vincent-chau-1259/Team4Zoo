using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zoo.Models;

namespace zoo.Controllers
{
  
    public class CareGiverHomeController : Controller
   {
        
        // GET: CareGiverHome
        public ActionResult Index()
        {
            IEnumerable<Animal> AnimalList = ViewMyAnimals();
            List<MyAnimal> Animals = new List<MyAnimal>();
            foreach (var item in AnimalList)
            {
                Animals.Add( new MyAnimal(item.animal_name, GetFamilyName(item.family),GetExihibitN(item.Exhibit_ID), GetExihibitL(item.Exhibit_ID)));
            }
            return View(Animals);
        }
        
        public IEnumerable<Animal> ViewMyAnimals()
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                System.Guid Employee_ID = (System.Guid)Session["Employee_ID"];
                return db.Animals.ToList().Where(x => (x.Assignee1_ID == Employee_ID || x.Assignee2_ID == Employee_ID || x.Assignee3_ID == Employee_ID));
            }
        }

        public String GetFamilyName(int? FamilyID)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                return db.Family_Name.Where(x => x.Family_ID == FamilyID).Select(y => y.family_title).FirstOrDefault();
            }

        }

        public String GetExihibitN(System.Guid ID)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                return db.Exhibits.Where(x => x.Exhibit_ID == ID).Select(y => y.exhibit_name).FirstOrDefault();
            }

        }

        public String GetExihibitL(System.Guid ID)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                return db.Exhibits.Where(x => x.Exhibit_ID == ID).Select(y => y.exhibit_loc).FirstOrDefault();
            }

        }
    }
}