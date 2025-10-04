using LoansPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Domain.Interfaces
{
    public interface IUserCache
    {
        Task<User?> GetUserAsync(Guid id);
        Task SaveUserAsync(User user);
        Task DeleteUserAsync(Guid id);
    }
}
