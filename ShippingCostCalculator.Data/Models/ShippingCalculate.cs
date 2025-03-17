using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCostCalculator.Data.Models
{
    public class ShippingCalculate
    {
        public required List<Product> Products { get; set; }
        public required Country Country { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
