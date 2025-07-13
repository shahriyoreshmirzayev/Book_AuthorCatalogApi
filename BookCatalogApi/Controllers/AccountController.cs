﻿using Application.Abstraction;
using Application.DTOs.UserDTO;
using Application.Extencions;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public AccountController(ITokenService tokenService, IUserRepository userRepository, IMapper mapper)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromForm] UserCredentials userCredentials)
    {
        var user = (await _userRepository.GetAsync(x => x.Password == userCredentials.Password.GetHash()
                                     && x.Email == userCredentials.Email)).FirstOrDefault();
        if (user != null)
        {
            RegisteredUserDTO userDTO = new()
            {
                User = user,
                UserTokens = await _tokenService.CreateTokensAsync(user)
            };
            return Ok(userDTO);
        }
        return BadRequest("login or password is incorrect!");
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Create([FromBody] UserCreateDTO NewUser)
    {
        if (ModelState.IsValid)
        {
            User user = _mapper.Map<User>(NewUser);
            user.Password = user.Password.GetHash();
            user = await _userRepository.AddAsync(user);
            if (user != null)
            {
                RegisteredUserDTO userDTO = new()
                {
                    User = user,
                    UserTokens = await _tokenService.CreateTokensAsync(user)
                };

                return Ok(userDTO);
            }
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Refresh")]
    public async Task<IActionResult> Refresh([FromBody] Token tokens)
    {
        var principal = _tokenService.GetClaimsFromExpiredToken(tokens.AccessToken);
        string? email = principal.Identity?.Name;
        if (email == null)
        {
            return NotFound("Refresh token not found!");
        }
        RefreshToken? savedRefreshToken = _tokenService.Get(x => x.Email == email &&
                                                  x.RefreshTokenValue == tokens.RefreshToken)
                                                 .FirstOrDefault();

        if (savedRefreshToken == null)
        {
            return BadRequest("Refresh token or Access token invalid!");
        }
        if (savedRefreshToken.ExpiredDate < DateTime.UtcNow)
        {
            _tokenService.Delete(savedRefreshToken);
            return StatusCode(405, "Refresh token already expired please login again");
        }
        Token newTokens = await _tokenService.CreateTokensFromRefresh(principal, savedRefreshToken);

        return Ok(newTokens);

    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllUsers()
    {
        IQueryable<User> res = await _userRepository.GetAsync(x => true);
        return Ok(res);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        if (ModelState.IsValid)
        {
            user = await _userRepository.UpdateAsync(user);
            return Ok(user);
        }
        return BadRequest();
    }
}