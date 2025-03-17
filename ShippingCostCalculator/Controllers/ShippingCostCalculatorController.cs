using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShippingCostCalculator.Data.Interface;
using ShippingCostCalculator.Data.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShippingCostCalculator.Controllers
{
    [Route("api/shipping")]
    [ApiController]
    public class ShippingCostCalculatorController : ControllerBase
    {
        readonly IShippingCalculatorService _shippingCalculatorService;
        private readonly ILogger<ShippingCostCalculatorController> _logger;
        public ShippingCostCalculatorController(IShippingCalculatorService shippingCalculatorService, ILogger<ShippingCostCalculatorController> logger)
        {
            _shippingCalculatorService = shippingCalculatorService;
            _logger = logger;
        }

        [HttpPost]
        [Route("calculate")]
        public async Task<IActionResult> Calculate([FromBody] ShippingCalculate shippingCalculate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _shippingCalculatorService.CalculateShippingCosts(shippingCalculate);
                if (!response.Success)
                {
                    return BadRequest(new { message = response.Message });
                }
                return Ok(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in country API");
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error please try latter");
            }
        }
    }
}
