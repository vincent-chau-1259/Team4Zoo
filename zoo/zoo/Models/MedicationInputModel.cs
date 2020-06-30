using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zoo.Models
{
    public class MedicationInputModel
    {
      //  public IEnumerable<Animal> animal { get; set; }
        [Required]
        public string Animal_name { get; set; }
        [Required]
        public string medication { get; set; }
        [Required]
        public string dose { get; set; }
        public string vet { get; set; }
        public string description { get; set; }
        
     
    }
}