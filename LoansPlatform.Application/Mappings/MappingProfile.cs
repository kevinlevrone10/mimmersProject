using AutoMapper;
using LoansPlatform.Application.DTOs.loanDtos;
using LoansPlatform.Application.DTOs.userDtos;
using LoansPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoansPlatform.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();

            // Loan
            CreateMap<Loan, LoanDto>().ReverseMap();
            CreateMap<CreateLoanDto, Loan>();
        }
    }
}
