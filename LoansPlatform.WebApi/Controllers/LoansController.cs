using LoansPlatform.Application.DTOs.loanDtos;
using LoansPlatform.Application.Services;
using LoansPlatform.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoansPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LoanService _loanService;

        public LoansController(LoanService loanService)
        {
            _loanService = loanService;
        }

        // GET: api/loans/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<LoanDto>> GetLoanById(Guid id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        // GET: api/loans/user/{userId}?status=Paid
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoansByUser(Guid userId, [FromQuery] LoanStatus? status)
        {
            var loans = await _loanService.GetByUserAsync(userId, status);
            return Ok(loans);
        }

        // POST: api/loans
        [HttpPost]
        public async Task<ActionResult<LoanDto>> CreateLoan([FromBody] CreateLoanDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loan = await _loanService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetLoanById), new { id = loan.Id }, loan);
        }

        // PUT: api/loans/{id}/pay
        [HttpPut("{id:guid}/pay")]
        public async Task<IActionResult> PayLoan(Guid id)
        {
            var success = await _loanService.PayLoanAsync(id);
            if (!success) return BadRequest("Loan not found or already paid.");
            return NoContent();
        }
    }
}
