using LoansPlatform.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Application.DTOs.loanDtos
{
    public class LoanDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public LoanStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
