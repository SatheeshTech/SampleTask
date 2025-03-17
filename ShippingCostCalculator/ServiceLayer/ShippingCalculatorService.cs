using ShippingCostCalculator.Data.Interface;
using ShippingCostCalculator.Data.Models;

namespace ShippingCostCalculator.ServiceLayer
{
    public class ShippingCalculatorService : IShippingCalculatorService
    {
        readonly IShippingCostCalculatorRepository _context;
        public ShippingCalculatorService(IShippingCostCalculatorRepository context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<ShippingCalculate>> CalculateShippingCosts(ShippingCalculate shippingCalculate)
        {

            var response = new ServiceResponse<ShippingCalculate>();
            foreach (var product in shippingCalculate.Products)
            {
                var dbProduct = await _context.SearchProductAsync(product.ProductId);
                if (dbProduct == null)
                {
                    response.Success = false;
                    response.Message = $"In valid Product: {product.ProductName}";
                    return response;
                }
                product.BasePrice = dbProduct.BasePrice;
            }
            var dbCountry = await _context.SearchCountryAsync(shippingCalculate.Country.CountryId);
            if (dbCountry == null)
            {
                response.Success = false;
                response.Message = $"Invalid Country: {shippingCalculate.Country.CountryName}";
                return response;
            }
            shippingCalculate.Country.BaseShippingCost = dbCountry.BaseShippingCost;
            shippingCalculate.Country.TaxRate = dbCountry.TaxRate;
            response.Data = ShippingCalculate(shippingCalculate);
            return response;


        }
        private ShippingCalculate ShippingCalculate(ShippingCalculate shippingCalculate)
        {
            decimal subTotal = shippingCalculate.Products.Sum(p => p.Quantity * p.BasePrice);
            decimal taxAmount = (subTotal * shippingCalculate.Country.TaxRate) / 100;
            decimal totalAmount = subTotal + taxAmount + shippingCalculate.Country.BaseShippingCost;

            shippingCalculate.SubTotal = Math.Round(subTotal, 2);
            shippingCalculate.TaxAmount = Math.Round(taxAmount, 2);
            shippingCalculate.TotalAmount = Math.Round(totalAmount, 2);

            return shippingCalculate;
        }
    }
}
