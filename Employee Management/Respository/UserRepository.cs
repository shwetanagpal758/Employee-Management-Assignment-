﻿using Employee_Management.Data;
using Employee_Management.Models;
using Employee_Management.Models.Dto;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee_Management.Respository
{
    public class UserRepository : IUserRepository

    {
        private readonly ApplicationDb _db;
        private string secretKey;
        public UserRepository(ApplicationDb db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName == username);


            if (user == null)
            {
                return true;

            }
            return false;
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
        && u.Password == loginRequestDTO.Password);

            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null

                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDTO = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user

            };
            return loginResponseDTO;
        }

            public async Task<LocalUser> Register(RegistrationRequestDto registrationRequestDTO)
            {
                LocalUser user = new LocalUser()
                {
                    UserName = registrationRequestDTO.UserName,
                    Password = registrationRequestDTO.Password,
                    Name = registrationRequestDTO.Name,
                    Role = registrationRequestDTO.Role

                };
            _db.LocalUsers.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
        }

    }

