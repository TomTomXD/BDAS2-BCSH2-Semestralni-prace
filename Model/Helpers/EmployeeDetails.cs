using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model
{
    public class EmployeeDetails
    {
        public string Department { get; set; }
        public string Position { get; set; }
        public Employee? Manager { get; set; }
    }
}
