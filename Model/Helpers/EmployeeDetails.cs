using FinancniInformacniSystemBanky.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model
{
    public class EmployeeDetails
    {
        public Department Department { get; set; }
        public Position Position { get; set; }
        public Employee? Manager { get; set; }
    }
}
