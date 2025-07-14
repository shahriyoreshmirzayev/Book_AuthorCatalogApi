﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.UserDTO;

public class UserGetDTO
{
    public int Id { get; set; }
    public string FullName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public int[] RolesId { get; set; }
}
