using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class AuthorRepository : IAuthorRepository
{
    private readonly IBookCatalogDbContext _bookCatalogDbContext;

    public AuthorRepository(IBookCatalogDbContext bookCatalogDbContext)
    {
        _bookCatalogDbContext = bookCatalogDbContext;
    }

    public async Task<Author> AddAsync(Author author)
    {
        _bookCatalogDbContext.Authors.Add(author);
        int res = await _bookCatalogDbContext.SaveChangesAsync();
        if (res > 0)
        {
            return author;
        }
        return null;
    }

    public async Task<IEnumerable<Author>?> AddRangeAsync(IEnumerable<Author> authors)
    {
        _bookCatalogDbContext.Authors.AttachRange(authors);

        int res = await _bookCatalogDbContext.SaveChangesAsync();

        if (res > 0) { return authors; }

        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Author? author = await _bookCatalogDbContext.Authors.FindAsync(id);
        if (author != null)
        {
            _bookCatalogDbContext.Authors.Remove(author);
        }
        int res = await _bookCatalogDbContext.SaveChangesAsync();
        if (res > 0)
            return true;
        return false;
    }

    public Task<IQueryable<Author>> GetAsync(Expression<Func<Author, bool>> predicate)
    {
        return Task.FromResult(_bookCatalogDbContext.Authors.Where(predicate));
    }

    public async Task<Author?> GetByIdAsync(int Id)
    {
        return await _bookCatalogDbContext.Authors.FindAsync(Id);
    }

    public async Task<Author?> UpdateAsync(Author author)
    {
        _bookCatalogDbContext.Authors.Update(author);

        int res = await _bookCatalogDbContext.SaveChangesAsync();
        if (res > 0) { return author; }

        return null;

    }
}
