using LoansPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Domain.Interfaces
{
    public interface ILoanCache
    {
        Task SaveLoanAsync(Loan loan);
        Task<Loan?> GetLoanAsync(Guid id);
        Task DeleteLoanAsync(Guid id);
    }
}
