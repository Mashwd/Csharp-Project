using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Entitites
{
    class Chart
    {
        public int ChartID{ get; set; }
        public string Product{ get; set; }
        public int Quantity{ get; set; }
        public double Discount{ get; set; }
        public double Price{ get; set; }
    }
}
