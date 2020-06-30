namespace zoo.Models

{

    using System;

    using System.Collections.Generic;

    using System.ComponentModel;

    using System.ComponentModel.DataAnnotations;



    public partial class Credential

    {

        public Credential()

        {

            this.Employees = new HashSet<Employee>();

        }

        [Required(ErrorMessage = "This field is required.")]

        [DisplayName("Username ")]

        public string username { get; set; }

        [DisplayName("Password ")]

        [DataType(DataType.Password)]

        [Required(ErrorMessage = "This field is required.")]

        public string password { get; set; }

        public System.Guid Employee_ID { get; set; }

        public string LoginErrorMessage { get; set; }
        
        public virtual ICollection<Employee> Employees { get; set; }

    }

}