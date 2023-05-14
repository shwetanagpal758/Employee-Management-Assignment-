using Employee_Management.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management.Controllers.Models
{
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Designationcode { get; set; }
        [ForeignKey("Employees")]
        public int EmployeesID { get; set; }
        public Employees Employees { get; set; }


        public string Designationname { get; set; }
    }
}
