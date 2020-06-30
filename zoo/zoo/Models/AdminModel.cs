using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core;

namespace zoo.Models
{
    public class AdminModel
    {
    }


    public class DeptChartViewModel
    {
        public string name { get; set; }
        public decimal revenue { get; set; }
        public decimal expenditure { get; set; }
    }

    public class DepartmentListModel
    {
        public string department_name { get; set; }
        public Guid Department_ID { get; set; }
    }

    public class RoleListModel
    {
        public string Job_Title { get; set; }
        public Guid Role_ID { get; set; }
    }
    
    //View Model for Employee List
    public class EmployeeViewModel
    {
        public string f_name { get; set; }//
        public string mid_init { get; set; }//
        public string l_name { get; set; }//
        public string email { get; set; }//
        public string sex { get; set; }//
        public string dob { get; set; }//
        public string ssn { get; set; }
        public decimal hourly_wage { get; set; }//
        public string hire_date { get; set; }
        public int? sick_days { get; set; }//
        public string phone_num { get; set; }//
        public string isActive { get; set; }
        public Guid? Employee_ID { get; set; }//
        public string department_name { get; set; }//
        public Guid? Department_ID { get; set; }//
        public string Job_Title { get; set; }//
        public Guid? Role_ID { get; set; }//
    }

    


}