using Domain.Enums;

namespace Application.DTOs.AuthorDTO
{
    public class AuthorGetDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public int[] BooksId { get; set; }
    }
}
