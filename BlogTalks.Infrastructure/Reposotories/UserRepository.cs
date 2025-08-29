using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Reposotories;
using BlogTalks.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BlogTalks.Infrastructure.Reposotories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context, context.Users) 
        {
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public User? GetByUsername(string Username)
        {
            return _context.Users.Where(u => u.Username == Username).FirstOrDefault();        }

        public IEnumerable<User> GetUsersByIds(IEnumerable<int> ids)
        {
            return _context.Users
                .Where(u => ids.Contains(u.Id));
        }
    }
}
