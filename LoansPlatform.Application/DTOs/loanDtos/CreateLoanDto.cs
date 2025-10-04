using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Application.DTOs.loanDtos
{
    public class CreateLoanDto
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
