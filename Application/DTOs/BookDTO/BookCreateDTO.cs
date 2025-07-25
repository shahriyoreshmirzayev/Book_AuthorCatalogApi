﻿using Domain.Enums;

namespace Application.DTOs.BookDTO;

public class BookCreateDTO
{
    public string Name { get; set; }

    public string ISBN { get; set; }

    public string? Description { get; set; }

    public DateTime PublishDate { get; set; }

    public int[] AuthorsId { get; set; }

    public BookCategory Category { get; set; }
}
