using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zoo.Models
{
    public class AddAnimal
    {
        public string name { get; set; }
        public string familyN { get; set; }
        public DateTime DOB { get; set; }
        public string sex { get; set; }
        public decimal weight { get; set; }
        public string owner { get; set; }
        public string ExhibitN { get; set; }
        public string AttrN { get; set; }
        public string Assignee1 { get; set; }
        public string Assignee2 { get; set; }
        public string Assginee3 { get; set; }

        public IEnumerable<string> emplyeeUserNames { get; set; }
        public IEnumerable<string> familyNames { get; set; }
        public IEnumerable<string> exhibitNames { get; set; }
        public IEnumerable<string> attrNames { get; set; }
        public IEnumerable<Char> sexs { get; set; }
        
        public string ErrorMessage1 { get; set; }
        public string ErrorMessage2 { get; set; }
    }
}