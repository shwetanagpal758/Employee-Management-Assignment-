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
    [Route("api/DesignationAPI")]
    [ApiController]
    public class DesignationAPIController : ControllerBase

    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        public DesignationAPIController(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]//getalluser
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DesignationDto>>> GetDesignations()
        {
           
            IEnumerable<Designation> DesignationList = await _db.Designations.ToListAsync();
            return Ok(_mapper.Map<List<DesignationDto>>(DesignationList));
        }

        [HttpGet("id")]//getuser
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DesignationDto>> GetDesignation(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var designation = await _db.Designations.FirstOrDefaultAsync(u => u.Designationcode == id);
            if (designation == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<DesignationDto>(designation));
        }

        [HttpPost]//post
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DesignationDto>> CreateDesignation([FromBody] DesignationDto designationDto)
        {
            if (!ModelState.IsValid)//(this is used for required method if that item is null then modelstate will give error,ex-name is req)
            {
                return BadRequest(ModelState);
            }
            if (await _db.Designations.FirstOrDefaultAsync(u => u.Designationcode == designationDto.Designationcode) != null)
            {
                ModelState.AddModelError("CustomError", "Email already Exists");//it will show if already exist is typed
                return BadRequest(ModelState);
            }
            if (designationDto == null)
            {
                return BadRequest(designationDto);
            }

          /* if (Designation.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/

            Designation model = _mapper.Map<Designation>(designationDto);//we do not want to write below code because one line is enough and that is called automapping
           /* * User model = new()
              {
                  Id = userDto.Id,
                  Name = userDto.Name,
                  Email = userDto.Email,
                  Password = userDto.Password,
                  Role = userDto.Role
              };*/
            await _db.Designations.AddAsync(model);
            await _db.SaveChangesAsync();

            return Ok(designationDto);



        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("id")]//delete
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var Designation = await _db.Designations.FirstOrDefaultAsync(u => u.Designationcode == id);
            if (Designation == null)
            {

                return NotFound();
            }
            _db.Designations.Remove(Designation);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPut("id")]//update
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDesignation(int id, [FromBody] DesignationDto DesignationDto)
        {
            if (DesignationDto == null || id != DesignationDto.Designationcode)
            {
                return BadRequest();
            }
            /*var user = UserStore.userList.FirstOrDefault(u =>u.Id == id);
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.Role = userDto.Role;*/

            Designation model = _mapper.Map<Designation>(DesignationDto);//automapping
            /* User model = new()
             {
                 Id = userDto.Id,
                 Name = userDto.Name,
                 Email = userDto.Email,
                 Password = userDto.Password,
                 Role = userDto.Role
             };*/
            _db.Designations.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
        }




    }
}
