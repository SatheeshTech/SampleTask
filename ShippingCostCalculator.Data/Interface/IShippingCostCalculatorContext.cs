using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShippingCostCalculator.Data.Models;

namespace ShippingCostCalculator.Data.Interface
{
    public interface IShippingCostCalculatorContext
    {
        DbSet<Country> Countries { get; set; }

        DbSet<Product> Products { get; set; }

        int SaveChanges();

    }
}
