using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management.Models.Dto
{
    public class DesignationDto
    {
        [Required]
        public int Designationcode { get; set; }
        [Required]
        public int EmployeesID { get; set; }
        
      
        public string Designationname { get; set; }
    }
}
