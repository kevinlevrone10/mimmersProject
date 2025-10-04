using AutoMapper;
using LoansPlatform.Application.DTOs.userDtos;
using LoansPlatform.Application.Mappings;
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
    public class UserServiceTests
    {
        private readonly IMapper _mapper;

        public UserServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenEmailIsEmpty()
        {
            // Arrange
            var repo = RepositoryMocks.GetUserRepository();
            var service = new UserService(repo.Object, _mapper , null , null);

            var dto = new CreateUserDto { Email = "", Password = "123456" };

            // Act
            Func<Task> act = async () => await service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("Email is required.");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenUserAlreadyExists()
        {
            // Arrange
            var repo = RepositoryMocks.GetUserRepository();
            var service = new UserService(repo.Object, _mapper , null, null);

            var dto = new CreateUserDto { Email = "existing@mail.com", Password = "123456" };

            // Act
            Func<Task> act = async () => await service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("A user with this email already exists.");
        }
    }
}
