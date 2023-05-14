using AutoMapper;
using Employee_Management.Controllers.Models.Dto;
using Employee_Management.Data;
using Employee_Management.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Controllers
{
    [Route("api/UserAPI")]
    [ApiController]
    public class UserAPIController : ControllerBase

    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public UserAPIController(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]//getalluser
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            IEnumerable<User> userList = await _db.Users.ToListAsync();
            return Ok(_mapper.Map<List<UserDto>>(userList));
        }

        [HttpGet("id")]//getuser
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]//post
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)//(this is used for required method if that item is null then modelstate will give error,ex-name is req)
            {
                return BadRequest(ModelState);
            }
            if (await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userDto.Email.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Email already Exists");//it will show if already exist is typed
                return BadRequest(ModelState);
            }
            if (userDto == null)
            {
                return BadRequest(userDto);
            }

            if (userDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            User model = _mapper.Map<User>(userDto);//we do not want to write below code because one line is enough and that is called automapping
            /*  User model = new()
              {
                  Id = userDto.Id,
                  Name = userDto.Name,
                  Email = userDto.Email,
                  Password = userDto.Password,
                  Role = userDto.Role
              };*/
            await _db.Users.AddAsync(model);
            await _db.SaveChangesAsync();

            return Ok(userDto);



        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("id")]//delete
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {

                return NotFound();
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("id")]//update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null || id != userDto.Id)
            {
                return BadRequest();
            }
            //var user = UserStore.userList.FirstOrDefault(u =>u.Id == id);
            /*user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Role = userDto.Role;*/

            User model = _mapper.Map<User>(userDto);//automapping
            /* User model = new()
             {
                 Id = userDto.Id,
                 Name = userDto.Name,
                 Email = userDto.Email,
                 Password = userDto.Password,
                 Role = userDto.Role
             };*/
            _db.Users.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }




    }
}
