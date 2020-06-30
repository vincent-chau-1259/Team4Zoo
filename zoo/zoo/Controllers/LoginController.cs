using zoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zoo.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            System.Guid Employee_ID = (System.Guid)Session["Employee_ID"];
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Authorize(Credential userModel)
        {
            using (team4zooEntities db = new team4zooEntities())
            {
                var userCredentials = db.Credentials.Where(x => x.username == userModel.username && x.password == userModel.password).FirstOrDefault();
                
                if (userCredentials != null) //Correct username/password
                {
                    var account_status = db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.isActive).FirstOrDefault();
                    if (userCredentials.Employee_ID == null || Convert.ToInt32(account_status) == 0 )
                    {
                        userModel.LoginErrorMessage = "The Account is deactivated";
                        return View("Index", userModel);
                    }
                    else 
                    {
                        Session["Name"] = db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.display_name).FirstOrDefault();
                        Session["username"] = userCredentials.username;
                        System.Guid userRoleID = (System.Guid)db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.Role_ID).FirstOrDefault();
                        Session["RoleName"] = db.Roles.Where(x => x.Role_ID == userRoleID).Select(y => y.Job_Title).FirstOrDefault();
                        Session["Employee_ID"] = userCredentials.Employee_ID;
                        Session["RoleID"] = userRoleID;
                        Session["FName"] = db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.f_name).FirstOrDefault();
                        Session["LName"] = db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.l_name).FirstOrDefault();
                        Session["Email"] = db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.email).FirstOrDefault();
                        Session["Phone"] = db.Employees.Where(x => x.Employee_ID == userCredentials.Employee_ID).Select(y => y.phone_num).FirstOrDefault();
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    userModel.LoginErrorMessage = "Wrong Username/Password";
                    return View("Index", userModel);
                }
            }
        }
    }
}