using Employee_Management.Controllers.Models.Dto;

namespace Employee_Management.Data
{
    public static class UserStore
    {
        public static List<UserDto> userList= new List<UserDto>
            {
                new UserDto{Id=1,Name="shweta",Email="shweta@gmail.com",Password="1234",Role="SoftwareEngineer"},
                 new UserDto{Id=2,Name="Ram",Email="ram@gmail.com",Password="1234",Role="SeniorSoftwareEngineer"},
            };
    }
}
