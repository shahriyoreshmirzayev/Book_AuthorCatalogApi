using Application.DTOs.RoleDTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    private readonly IMapper _mapper;
    public RoleController(IRoleRepository roleRepository, IMapper mapper, IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _permissionRepository = permissionRepository;
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetRoleById([FromQuery] int id)
    {
        Role role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
        {
            return NotFound($"Role Id:{id} not found!");
        }
        return Ok(role);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllRoles()
    {
        IQueryable<Role> Roles = await _roleRepository.GetAsync(x => true);

        return Ok(Roles);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDTO roleCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Role role = _mapper.Map<Role>(roleCreateDto);
        List<Permission> permissions = new();
        for (int i = 0; i < role.Permissions.Count; i++)
        {
            Permission permission = role.Permissions.ToArray()[i];

            permission = await _permissionRepository.GetByIdAsync(permission.PermissionId);
            if (permission == null)
            {
                return NotFound($"Permission not found");
            }
            permissions.Add(permission);
        }
        role.Permissions = permissions;
        role = await _roleRepository.AddAsync(role);
        if (role == null) return BadRequest(ModelState);

        RoleGetDTO roleGet = _mapper.Map<RoleGetDTO>(role);
        return Ok(roleGet);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDTO roleUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Role role = _mapper.Map<Role>(roleUpdateDto);

        role = await _roleRepository.UpdateAsync(role);
        if (role == null) return BadRequest(ModelState);

        RoleGetDTO roleGet = _mapper.Map<RoleGetDTO>(role);
        return Ok(roleGet);


    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteRole([FromQuery] int id)
    {
        bool isDelete = await _roleRepository.DeleteAsync(id);
        return isDelete ? Ok("Deleted successfully")
            : BadRequest("Delete operation failed");
    }
}
