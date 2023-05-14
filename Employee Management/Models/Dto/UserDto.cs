using System.ComponentModel.DataAnnotations;

namespace Employee_Management.Controllers.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
