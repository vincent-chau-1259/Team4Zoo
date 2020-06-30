using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zoo.Models
{
    public struct MyAnimal
    {
        public string Name;
        public string Family;
        public string ExihibitionN;
        public string ExihibitionL;

        public MyAnimal(string name, string fam, string exn, string exl)
        {
            Name = name; Family = fam; ExihibitionN = exn; ExihibitionL = exl;
        }
    }
}