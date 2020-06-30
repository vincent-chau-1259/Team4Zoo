using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zoo.Models
{
    public struct MyEmployee
    {
        public string FullName;
        public string fname;
        public string lname;
        public string email;
        public string phone;

        public MyEmployee(string f_name, string l_name, string email_add, string phone_num)
        {
            fname = f_name; lname = l_name; email = email_add; phone = phone_num; FullName = fname + " " + lname;
        }
    }
}