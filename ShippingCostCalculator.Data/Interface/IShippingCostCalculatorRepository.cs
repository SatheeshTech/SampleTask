using ShippingCostCalculator.Data.Models;

namespace ShippingCostCalculator.Data.Interface
{
    public interface IShippingCostCalculatorRepository
    {
        Task<IEnumerable<Country>> GetCountriesAsync();
        Task<IEnumerable<Product>> GetProductsAsync(int pageNumber = 1, int pageSize = 10);
        Task<Product?> SearchProductAsync(int productId);
        Task<Country?> SearchCountryAsync(int countryId);
    }
}
