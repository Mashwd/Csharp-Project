using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Entitites
{
    class Transition
    {
        public int TransitionID { get; set; }
        public string Details { get; set; }
        public double TotalPrice  { get; set; }
        public string Time { get; set; }
    }
}
