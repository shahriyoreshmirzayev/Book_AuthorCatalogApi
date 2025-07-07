using Application.Abstraction;
using Application.Extencions;
using Application.Models;
using Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRrepository _userRepository;
    public AccountController(ITokenService tokenService, IUserRrepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    [HttpGet]
    public IActionResult Login([FromQuery] UserCredentials userCredentials)
    {
        var user = _userRepository.GetAsync(x => x.Password.GetHash() == userCredentials.Password.GetHash()
                                                && x.Email == userCredentials.Email);
        if (Login != null)
            return Ok(_tokenService.CreateToken());
        return BadRequest("Login in password is incorrect!");
    }
}
