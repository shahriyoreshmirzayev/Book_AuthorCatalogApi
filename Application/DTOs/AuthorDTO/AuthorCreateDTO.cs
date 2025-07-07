﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthorDTO
{
    public class AuthorCreateDTO
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

    }

}
