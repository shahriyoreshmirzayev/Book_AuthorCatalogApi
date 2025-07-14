using Application.DTOs.UserDTO;
using Application.Extencions;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _permissionRepository;

        private readonly IMapper _mapper;

        public UserController(IRoleRepository permissionRepository, IUserRepository userRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User Id:{id} not found!");
            }
            return Ok(user);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            IQueryable<User> Users = await _userRepository.GetAsync(x => true);

            return Ok(Users);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = _mapper.Map<User>(userCreateDto);
            List<Domain.Entities.Role> permissions = new();
            for (int i = 0; i < user.Roles.Count; i++)
            {
                Role permission = user.Roles.ToArray()[i];

                permission = await _permissionRepository.GetByIdAsync(permission.RoleId);
                if (permission == null)
                {
                    return NotFound($"Role not found");
                }
                permissions.Add(permission);
            }
            user.Roles = permissions;
            user = await _userRepository.AddAsync(user);
            if (user == null) return BadRequest(ModelState);

            UserGetDTO userGet = _mapper.Map<UserGetDTO>(user);
            return Ok(userGet);


        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = _mapper.Map<User>(userUpdateDto);

            user = await _userRepository.UpdateAsync(user);
            if (user == null) return BadRequest(ModelState);

            UserGetDTO userGet = _mapper.Map<UserGetDTO>(user);
            return Ok(userGet);


        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            bool isDelete = await _userRepository.DeleteAsync(id);
            return isDelete ? Ok("Deleted successfully")
                : BadRequest("Delete operation failed");
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeUserPassword(UserChangePasswordDTO userChangePassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetByIdAsync(userChangePassword.UserId);

                if (user != null)
                {
                    string CurrentHash = userChangePassword.CurrentPassword.GetHash();
                    if (CurrentHash == user.Password
                        && userChangePassword.NewPassword == userChangePassword.ConfirmNewPassword)
                    {
                        user.Password = userChangePassword.NewPassword.GetHash();
                        await _userRepository.UpdateAsync(user);
                        return Ok();
                    }
                    else return BadRequest("Incorrect password");
                }
                return BadRequest("User not found");
            }
            return BadRequest(ModelState);
        }
    }
}
