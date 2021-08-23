using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Entitites
{
    class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCategory { get; set; }
        public string ProductStatus { get; set; }
        public double Discount { get; set; }
    }
}
