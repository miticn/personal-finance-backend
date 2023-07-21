using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Transaction.Models;
using Transaction.Services;

namespace Transaction.Controllers
{
    [Route("transactions")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ITransactionService productsService, ILogger<ProductsController> logger)
        {
            _transactionService = productsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery(Name ="transaction-kind")] string transactionKind,
            [FromQuery(Name = "start-date")] DateTime? startDate,
            [FromQuery(Name = "end-date")] DateTime? endDate,
            [FromQuery(Name = "sort-by")] string sortBy,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10,
            [FromQuery(Name = "sort-order")] SortOrder sortOrder = SortOrder.Asc)
        {
            var result = await _transactionService.GetTransactions(transactionKind, startDate, endDate, sortBy, page, pageSize, sortOrder);
            return Ok(result);
        }

        [HttpPost("import"), Consumes("application/csv")]
        public async Task<ActionResult> ImportTransactions()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    var csvData = await reader.ReadToEndAsync();
                    var transactions = await _transactionService.ParseCsv(csvData);
                    await _transactionService.ImportTransactions(transactions);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while importing transactions");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }
        [HttpPost("{id}/split")]
        public async Task<IActionResult> GetProduct([FromRoute] string id)
        {
            var product = await _transactionService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
