using Employee_Management.Controllers.Models;
using Employee_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Data
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options)
           : base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employees> Employeess { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }    
    }
}
