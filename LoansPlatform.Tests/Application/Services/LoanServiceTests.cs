using AutoMapper;
using LoansPlatform.Application.DTOs.loanDtos;
using LoansPlatform.Application.Services;
using LoansPlatform.Application.Utils;
using LoansPlatform.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Tests.Application.Services
{
    public class LoanServiceTests
    {
        private readonly IMapper _mapper;

        public LoanServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateLoanDto, CreateLoanDto>();
                cfg.CreateMap<LoanDto, LoanDto>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAmountIsNegative()
        {
            var loanRepo = RepositoryMocks.GetLoanRepository();
            var userRepo = RepositoryMocks.GetUserRepository();

            var service = new LoanService(loanRepo.Object,  _mapper, userRepo.Object,null );
            var dto = new CreateLoanDto { UserId = Guid.NewGuid(), Amount = -100 };

            Func<Task> act = async () => await service.CreateAsync(dto);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Loan amount must be a positive value.");
        }

        [Fact]
        public async Task PayLoanAsync_ShouldThrow_WhenLoanAlreadyPaid()
        {
            var loanRepo = RepositoryMocks.GetLoanRepository();
            var userRepo = RepositoryMocks.GetUserRepository();

            var service = new LoanService(loanRepo.Object, _mapper, userRepo.Object , null);
            var paidLoan = loanRepo.Object.GetByUserAsync(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"))
                                          .Result.First();

            Func<Task> act = async () => await service.PayLoanAsync(paidLoan.Id);

            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Paid loans are immutable and cannot be modified.");
        }
    }
}
