using Finance.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Transaction.Controllers;
using Transaction.Services;

namespace Finance.Controllers
{
    [Route("categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost("import"), Consumes("application/csv")]
        public async Task<ActionResult> ImportCategories()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    var csvData = await reader.ReadToEndAsync();
                    var categories = await _categoryService.ParseCsv(csvData);
                    await _categoryService.ImportCategories(categories);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while importing categories");
                return StatusCode(500, new { Error = "An unexpected error occurred." });
            }
        }
    }
}
