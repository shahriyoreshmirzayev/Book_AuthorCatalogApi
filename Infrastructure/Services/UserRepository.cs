using Application.Abstraction;
using Application.Repositories;
using Domain.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class UserRepository : IUserRrepository
    {
        private readonly IBookCatalogDbContext _bookCatalogDb;

        public UserRepository(IBookCatalogDbContext bookCatalogDb)
        {
            _bookCatalogDb = bookCatalogDb;
        }

        public async Task<User> AddAsync(User user)
        {
            _bookCatalogDb.Users.Add(user);
            int res = await _bookCatalogDb.SaveChangesAsync();
            if (res > 0) return user;
            return null;
        }
        public Task<IEnumerable<User>> AddRangeAsync(IEnumerable<User> sync)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? user = await _bookCatalogDb.Users.FindAsync(id);
            if (user != null)
            {
                _bookCatalogDb.Users.Remove(user);
                int res = await _bookCatalogDb.SaveChangesAsync();
                if (res > 0) return true;
            }
            return false;
        }

        public Task<IQueryable<User>> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return Task.FromResult(_bookCatalogDb.Users.Where(predicate));
        }

        public async Task<User> GetByIdAsync(int Id)
        {
            return await _bookCatalogDb.Users.FindAsync(Id);
        }

        public async Task<User> UpdateAsync(User user)
        {
            _bookCatalogDb.Users.Update(user);
            int res = await _bookCatalogDb.SaveChangesAsync();
            if (res > 0) return user;
            return null;
        }
    }
}
