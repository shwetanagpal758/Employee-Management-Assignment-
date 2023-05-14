using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management.Models
{
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Departmentcode { get; set; }

        [ForeignKey("Employees")]
        public int EmployeesID { get; set; }
        public Employees Employees { get; set; }


        public string Departmentname { get; set; }
    }
}
