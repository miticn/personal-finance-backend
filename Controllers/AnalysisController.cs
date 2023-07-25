using Finance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transaction.Controllers;
using Transaction.Database.Repositories;
using Transaction.Services;

namespace Finance.Controllers
{
    [ApiController]
    [Route("spending-analytics")]
    public class AnalysisController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;

        public AnalysisController(ITransactionService transactionService, ICategoryService categoryService,
            ITransactionRepository transactionRepository, ICategoryRepository categoryRepository,
            ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _logger = logger;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAnalytics(
            [FromQuery] string? catcode,
            [FromQuery(Name = "start-date")] DateTime? startDate,
            [FromQuery(Name = "end-date")] DateTime? endDate,
            [FromQuery] string? direction)
        {
            if (direction != null && (direction != "d" && direction != "c"))
            {
                return BadRequest("Direction must be 'd' or 'c'");
            }
            var result = await _transactionRepository.GetAnalytics(catcode, startDate, endDate, direction);


            return Ok(new { group = result });
        }
    }
}
