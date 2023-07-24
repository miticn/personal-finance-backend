﻿using Finance.Commands;
using Finance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Transaction.Database.Repositories;
using Transaction.Models;
using Transaction.Services;

namespace Transaction.Controllers
{
    [Route("transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionController> _logger;
        private readonly ICategoryService _categoryService;

        public TransactionController(ITransactionService transactionService, ICategoryService categoryService,
            ITransactionRepository transactionRepository, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _logger = logger;
            _transactionRepository = transactionRepository;
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
        [HttpPost("{id}/categorize")]
        public async Task<ActionResult> CategorizeTransaction(string id, [FromBody] TransactionCategorizeCommand categorizeCommand)
        {
            
            var transaction = await _transactionRepository.Get(id);
            var category = await _categoryService.GetCategory(categorizeCommand.catcode);

            if (transaction == null)
            {
                return NotFound("Transaction not found");
            }

            if (category == null)
            {
                return NotFound("Category not found");
            }

            transaction.Catcode = category.Code;

            try
            {
                await _transactionRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Error while saving changes");
            }

            return Ok("Transaction categorized");
        }

        [HttpPost("{id}/split")]
        public async Task<IActionResult> GetProduct([FromRoute] string id)
        {
            var product = await _transactionService.GetTransaction(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
