﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
     
    public string FullName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
}
