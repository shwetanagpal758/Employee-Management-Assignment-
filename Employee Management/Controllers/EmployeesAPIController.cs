using AutoMapper;
using Employee_Management.Controllers.Models;
using Employee_Management.Controllers.Models.Dto;
using Employee_Management.Data;
using Employee_Management.Models;
using Employee_Management.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Controllers
{
    [Route("api/EmployeesAPI")]
    [ApiController]
    public class EmployeesAPIController : ControllerBase

    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public EmployeesAPIController(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]//getalluser
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeesDto>>> GetEmployeess()
        {
           
            IEnumerable<Employees> EmployeesList = await _db.Employeess.ToListAsync();
            return Ok(_mapper.Map<List<EmployeesDto>>(EmployeesList));
        }
        [Authorize(Roles = "admin")]
        [HttpGet("id")]//getuser
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeesDto>> GetEmployees(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var employees = await _db.Employeess.FirstOrDefaultAsync(u => u.Id == id);
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EmployeesDto>(employees));
        }

        [HttpPost]//post
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeesDto>> CreateEmployees([FromBody] EmployeesDto employeesDto)
        {
            if (!ModelState.IsValid)//(this is used for required method if that item is null then modelstate will give error,ex-name is req)
            {
                return BadRequest(ModelState);
            }
            if (await _db.Employeess.FirstOrDefaultAsync(u => u.Name.ToLower() == employeesDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Email already Exists");//it will show if already exist is typed
                return BadRequest(ModelState);
            }
            if (employeesDto == null)
            {
                return BadRequest(employeesDto);
            }

            /* if (Designation.Id > 0)
              {
                  return StatusCode(StatusCodes.Status500InternalServerError);
              }*/

            Employees model = _mapper.Map<Employees>(employeesDto);//we do not want to write below code because one line is enough and that is called automapping
           /* User model = new()
              {
                  Id = userDto.Id,
                  Name = userDto.Name,
                  Email = userDto.Email,
                  Password = userDto.Password,
                  Role = userDto.Role
              };*/
            await _db.Employeess.AddAsync(model);
            await _db.SaveChangesAsync();

            return Ok(employeesDto);



        }

        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("id")]//delete
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var Employees = await _db.Employeess.FirstOrDefaultAsync(u => u.Id == id);
            if (Employees == null)
            {

                return NotFound();
            }
            _db.Employeess.Remove(Employees);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPut("id")]//update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployees(int id, [FromBody] EmployeesDto EmployeesDto)
        {
            if (EmployeesDto == null || id != EmployeesDto.Id)
            {
                return BadRequest();
            }
            /* var user = UserStore.userList.FirstOrDefault(u =>u.Id == id);
             user.Name = userDto.Name;
             user.Email = userDto.Email;
             user.Password = userDto.Password;
             user.Role = userDto.Role;*/

            Employees model = _mapper.Map<Employees>(EmployeesDto);//automapping
          //   User model = new()
            /* {
                 Id = userDto.Id,
                 Name = userDto.Name,
                 Email = userDto.Email,
                 Password = userDto.Password,
                 Role = userDto.Role
             };*/
            _db.Employeess.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }




    }
}
