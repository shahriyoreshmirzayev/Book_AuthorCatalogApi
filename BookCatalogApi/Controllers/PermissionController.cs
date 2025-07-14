using Application.DTOs.PermissionDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace BookCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        private readonly IMapper _mapper;
        public PermissionController(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPermissionById([FromQuery] int id)
        {

            Permission permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
            {
                return NotFound($"Permission Id:{id} not found!");
            }
            return Ok(permission);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPermissions()
        {
            IQueryable<Permission> Permissions = await _permissionRepository.GetAsync(x => true);

            return Ok(Permissions);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionCreateDTO permissionCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Permission permission = _mapper.Map<Permission>(permissionCreateDto);
            permission = await _permissionRepository.AddAsync(permission);
            if (permission == null) return BadRequest(ModelState);
            return Ok(permission);


        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePermission([FromBody] Permission PermissionUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Permission permission = _mapper.Map<Permission>(PermissionUpdateDto);

            permission = await _permissionRepository.UpdateAsync(permission);
            if (permission == null) return NotFound();

            return Ok(permission);


        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeletePermission([FromQuery] int id)
        {
            bool isDelete = await _permissionRepository.DeleteAsync(id);

            return isDelete ? Ok("Deleted successfully")
                : BadRequest("Delete operation failed");
        }

    }
}
