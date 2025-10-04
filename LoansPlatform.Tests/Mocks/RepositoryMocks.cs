using LoansPlatform.Domain.Entities;
using LoansPlatform.Domain.Enum;
using LoansPlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Tests.Mocks
{
    public class RepositoryMocks
    {
        public static Mock<IUserRepository> GetUserRepository()
        {
            var mock = new Mock<IUserRepository>();

            var users = new List<User>
            {
                new() { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Email = "existing@mail.com" }
            };

            mock.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email == email));

            mock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => users.FirstOrDefault(u => u.Id == id));

            mock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => users.Add(u));

            return mock;
        }

        public static Mock<ILoanRepository> GetLoanRepository()
        {
            var mock = new Mock<ILoanRepository>();

            var loans = new List<Loan>
            {
                new() { Id = Guid.NewGuid(), UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Amount = 5000, Status = LoanStatus.Paid }
            };

            mock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => loans.FirstOrDefault(l => l.Id == id));

            mock.Setup(r => r.GetByUserAsync(It.IsAny<Guid>(), null))
                .ReturnsAsync((Guid userId, LoanStatus? status) =>
                    loans.Where(l => l.UserId == userId && (!status.HasValue || l.Status == status.Value)).ToList());

            mock.Setup(r => r.AddAsync(It.IsAny<Loan>()))
                .Callback<Loan>(l => loans.Add(l));

            return mock;
        }
    }
}
