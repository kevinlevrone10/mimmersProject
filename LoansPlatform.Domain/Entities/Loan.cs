using LoansPlatform.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansPlatform.Domain.Entities
{
    public class Loan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }

        [Required]
        public LoanStatus Status { get; set; } = LoanStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public User? User { get; set; }

        public void MarkAsPaid()
        {
            if (Status == LoanStatus.Paid)
                throw new InvalidOperationException("Loan already paid.");
            Status = LoanStatus.Paid;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
