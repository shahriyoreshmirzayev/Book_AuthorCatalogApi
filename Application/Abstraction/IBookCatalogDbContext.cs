using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstraction;

public interface IBookCatalogDbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
}
