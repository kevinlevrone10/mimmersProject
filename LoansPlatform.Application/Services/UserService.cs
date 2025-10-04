using AutoMapper;
using LoansPlatform.Application.DTOs.userDtos;
using LoansPlatform.Application.Utils;
using LoansPlatform.Domain.Entities;
using LoansPlatform.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILoanRepository _loanRepository;
        private readonly IUserCache _userCache;

        public UserService(IUserRepository userRepository, IMapper mapper, ILoanRepository loanRepository , IUserCache userCache)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _loanRepository = loanRepository;
            _userCache = userCache;

        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var cachedUser = await _userCache.GetUserAsync(id);
            if (cachedUser != null)
                return _mapper.Map<UserDto>(cachedUser);
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new BusinessException($"User with ID {id} not found.");

            await _userCache.SaveUserAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new BusinessException("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new BusinessException("Password must be at least 6 characters long.");

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new BusinessException("A user with this email already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddAsync(user);

            
            await _userCache.SaveUserAsync(user);
            
            

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new BusinessException("User not found.");

            var userLoans = await _loanRepository.GetByUserAsync(id);
            if (userLoans.Any())
                throw new BusinessException("Cannot delete user with existing loans.");

            await _userRepository.DeleteAsync(id);
            await _userCache.DeleteUserAsync(id);
        }
    }
}
