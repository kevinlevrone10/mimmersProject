using AutoMapper;
using LoansPlatform.Application.DTOs.loanDtos;
using LoansPlatform.Domain.Entities;
using LoansPlatform.Domain.Enum;
using LoansPlatform.Domain.Interfaces;
using LoansPlatform.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Application.Services
{
    public class LoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly ILoanCache _loanCache;
        private readonly IUserRepository _userRepository;

        public LoanService(ILoanRepository loanRepository, IMapper mapper, IUserRepository userRepository, ILoanCache loanCache)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _loanCache = loanCache;
        }

        public async Task<LoanDto?> GetByIdAsync(Guid id)
        {
            var cachedLoan = await _loanCache.GetLoanAsync(id);
            if (cachedLoan != null)
                return _mapper.Map<LoanDto>(cachedLoan);


            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
                throw new BusinessException($"Loan with ID {id} not found.");

            await _loanCache.SaveLoanAsync(loan);


            return _mapper.Map<LoanDto>(loan);
        }

        public async Task<IEnumerable<LoanDto>> GetByUserAsync(Guid userId, LoanStatus? status = null)
        {
            var loans = await _loanRepository.GetByUserAsync(userId, status);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
        {

            if (dto.Amount <= 0)
                throw new BusinessException("Loan amount must be a positive value.");

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new BusinessException("User not found.");



            // Rule: User cannot have an outstanding loan
            var userLoans = await _loanRepository.GetByUserAsync(dto.UserId);
            if (userLoans.Any(l => l.Status == LoanStatus.Pending))
                throw new BusinessException("User already has a pending loan.");

            var loan = _mapper.Map<Loan>(dto);
            loan.Id = Guid.NewGuid();

            await _loanRepository.AddAsync(loan);

            await _loanCache.SaveLoanAsync(loan);

            return _mapper.Map<LoanDto>(loan);
        }

        public async Task<bool> PayLoanAsync(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
                throw new BusinessException("Loan not found.");

            if (loan.Status == LoanStatus.Paid)
                throw new BusinessException("Paid loans are immutable and cannot be modified.");

            loan.MarkAsPaid();
            await _loanRepository.UpdateAsync(loan);

            await _loanCache.SaveLoanAsync(loan);

            return true;
        }



    }
}