using System.ComponentModel.DataAnnotations;

namespace Employee_Management.Models.Dto
{
    public class DepartmentDto
    {
        [Required]
        public int Departmentcode { get; set; }
        [Required]
        public int EmployeesID { get; set; }

        public string Departmentname { get; set; }
    }
}
