using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShippingCostCalculator.Data.Interface;
using ShippingCostCalculator.Data.Models;


namespace ShippingCostCalculator.Controllers
{
    [Route("api")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IShippingCostCalculatorRepository _context;
        private readonly ILogger<MasterDataController> _logger;

        public MasterDataController(IShippingCostCalculatorRepository context, ILogger<MasterDataController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("master/countries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var data = await _context.GetCountries();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in country API");
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error please try latter");
            }
        }

        [HttpGet]
        [Route("master/products")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var data = await _context.GetProducts();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Product API");
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error please try latter");
            }
        }
    }
}
