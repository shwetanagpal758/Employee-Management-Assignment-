using Employee_Management.Models.Dto;
using Employee_Management.Models;

namespace Employee_Management.Respository
{
    
        public interface IUserRepository
        {
            bool IsUniqueUser(string username);
            Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);

            Task<LocalUser> Register(RegistrationRequestDto registrationRequestDTO);

        }
    
}
