using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Entitites
{
    class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public int NoOfProducts { get; set; }
        public string BrandStatus{ get; set; }
        public double Discount { get; set; }
    }
}
