using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BlogTalks.Application.Abstractions
{
    public interface IAuthService
    {
        public string Create(BlogTalks.Domain.Entities.User user);
    }
}
