using AutoMapper;
using Employee_Management.Controllers.Models;
using Employee_Management.Controllers.Models.Dto;
using Employee_Management.Data;
using Employee_Management.Models;
using Employee_Management.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Controllers
{
    [Route("api/DepartmentAPI")]
    [ApiController]
    public class DepartmentAPIController : ControllerBase

    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public DepartmentAPIController(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]//getalluser
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> Departments()
        {
           
            IEnumerable<Department> DepartmentList = await _db.Departments.ToListAsync();
            return Ok(_mapper.Map<List<DepartmentDto>>(DepartmentList));
        }

        [HttpGet("id")]//getuser
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var Department = await _db.Departments.FirstOrDefaultAsync(u => u.Departmentcode == id);
            if (Department == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<DepartmentDto>(Department));
        }

        [HttpPost]//post
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] DepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)//(this is used for required method if that item is null then modelstate will give error,ex-name is req)
            {
                return BadRequest(ModelState);
            }
            if (await _db.Departments.FirstOrDefaultAsync(u => u.Departmentcode == departmentDto.Departmentcode) != null)
            {
                ModelState.AddModelError("CustomError", "Departmentcode already Exists");//it will show if already exist is typed
                return BadRequest(ModelState);
            }
            if (departmentDto == null)
            {
                return BadRequest(departmentDto);
            }

            /* if (Designation.Id > 0)
              {
                  return StatusCode(StatusCodes.Status500InternalServerError);
              }*/

            Department model = _mapper.Map<Department>(departmentDto);//we do not want to write below code because one line is enough and that is called automapping
           /* * User model = new()
              {
                  Id = userDto.Id,
                  Name = userDto.Name,
                  Email = userDto.Email,
                  Password = userDto.Password,
                  Role = userDto.Role
              };*/
            await _db.Departments.AddAsync(model);
            await _db.SaveChangesAsync();

            return Ok(departmentDto);



        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("id")]//delete
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var Department = await _db.Departments.FirstOrDefaultAsync(u => u.Departmentcode == id);
            if (Department == null)
            {

                return NotFound();
            }
            _db.Departments.Remove(Department);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPut("id")]//update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto departmentDto)
        {
            if (departmentDto == null || id != departmentDto.Departmentcode)
            {
                return BadRequest();
            }
            /*var user = UserStore.userList.FirstOrDefault(u =>u.Id == id);
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Role = userDto.Role;*/

            Department model = _mapper.Map<Department>(departmentDto);//automapping
            /* User model = new()
             {
                 Id = userDto.Id,
                 Name = userDto.Name,
                 Email = userDto.Email,
                 Password = userDto.Password,
                 Role = userDto.Role
             };*/
            _db.Departments.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }




    }
}
