using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUDWithWebAPI.Models
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage ="Name Cannot be Blank")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Country is not selected")]
        public string Country { get; set; }

        [Required(ErrorMessage = "State is not selected")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is not selected")]
        public string City { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        public DateTime DOB { get; set; }
    }
}