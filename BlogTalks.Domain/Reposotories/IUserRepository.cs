using BlogTalks.Domain.Entities;
using BlogTalks.Infrastructure.Reposotories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Reposotories
{
    public interface IUserRepository : IRepository<User>
    {
        public User GetByEmail(string Email);
        public User GetByUsername(string Username);
        public IEnumerable<User> GetUsersByIds(IEnumerable<int> ids);
    }
}























