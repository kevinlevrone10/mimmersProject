using LoansPlatform.Domain.Entities;
using LoansPlatform.Domain.Enum;
using LoansPlatform.Domain.Interfaces;
using LoansPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Loan?> GetByIdAsync(Guid id)
        {
            return await _context.Loans
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Loan>> GetByUserAsync(Guid userId, LoanStatus? status = null)
        {
            var query = _context.Loans
                .Where(l => l.UserId == userId);

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            return await query.ToListAsync();
        }

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null && loan.Status != LoanStatus.Paid)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }


    }
}
