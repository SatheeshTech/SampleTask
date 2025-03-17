using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShippingCostCalculator.Data.Interface;
using ShippingCostCalculator.Data.Models;
using ShippingCostCalculator.Helpers;

namespace ShippingCostCalculator.Data.Repository
{
    public class ShippingCostCalculatorRepository : IShippingCostCalculatorRepository
    {
        readonly IShippingCostCalculatorContext _context;
        readonly IMemoryCache _memoryCache;
        private const string countryCacheKey = "CountryList";
        public ShippingCostCalculatorRepository(IShippingCostCalculatorContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            if (_memoryCache.TryGetValue(countryCacheKey, out IEnumerable<Country> countries))
            {
                return countries;
            }
            else
            {
                var data = await _context.Countries.ToListAsync();
                _memoryCache.SetWithDefaults(countryCacheKey, data);
                return data;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            return await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.ProductId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Product?> SearchProductAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }
        public async Task<Country?> SearchCountryAsync(int countryId)
        {
            return await _context.Countries.FindAsync(countryId);
        }
    }
}
