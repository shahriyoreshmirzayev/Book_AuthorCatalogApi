using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class BookCatalogDbContext : DbContext, IBookCatalogDbContext
{
    public BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options)
        : base(options)
    {

    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

}
