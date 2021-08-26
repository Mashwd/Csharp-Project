using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Entitites
{
    class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public double Salary { get; set; }
        public string JoiningDate { get; set; }
    }
}
