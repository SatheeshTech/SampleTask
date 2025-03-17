using ShippingCostCalculator.Data.Models;

namespace ShippingCostCalculator.Data.Interface
{
    public interface IShippingCalculatorService
    {
         Task<ServiceResponse<ShippingCalculate>> CalculateShippingCosts(ShippingCalculate shippingCalculate);
    }
}
