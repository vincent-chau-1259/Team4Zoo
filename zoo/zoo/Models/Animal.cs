//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace zoo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Animal
    {
        public Animal()
        {
            this.Animal_Feed_Care = new HashSet<Animal_Feed_Care>();
            this.Animal_Medication_Care = new HashSet<Animal_Medication_Care>();
            this.Employees = new HashSet<Employee>();
            this.Food_Supply = new HashSet<Food_Supply>();
        }

        public System.Guid Animal_ID { get; set; }
        public string animal_name { get; set; }
        public Nullable<int> family { get; set; }
        public System.DateTime? dob { get; set; }
        public string sex { get; set; }
        public decimal weight { get; set; }
        public string owner { get; set; }
        public System.Guid Exhibit_ID { get; set; }
        public Nullable<System.Guid> Attraction_ID { get; set; }
        public bool isActive { get; set; }
        
        public string medication { get; set; }
        
        public string dose { get; set; }
        
        public string vet { get; set; }
        
        public string description { get; set; }
        public System.DateTime from { get; set; }
        public System.DateTime to { get; set; }
        public Nullable<System.Guid> Assignee1_ID { get; set; }
        public Nullable<System.Guid> Assignee2_ID { get; set; }
        public Nullable<System.Guid> Assignee3_ID { get; set; }

        public virtual ICollection<Animal_Feed_Care> Animal_Feed_Care { get; set; }
        public virtual Exhibit Exhibit { get; set; }
        public virtual Family_Name Family_Name { get; set; }
        public virtual ICollection<Animal_Medication_Care> Animal_Medication_Care { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Food_Supply> Food_Supply { get; set; }
       

        public string assignee1_name { get; set; }
        public string assignee2_name { get; set; }
        public string assignee3_name { get; set; }

        public override string ToString()
        {
            return this.animal_name;
        }

    }
}
