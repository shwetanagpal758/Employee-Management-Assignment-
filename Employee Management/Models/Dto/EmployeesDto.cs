using System.ComponentModel.DataAnnotations;

namespace Employee_Management.Models.Dto
{
    public class EmployeesDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Qualification { get; set; }



    }
}
