using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model.Helpers
{
    public class EmployeeNode
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName} ({Position})";
        public string Position { get; set; }
        public string Department { get; set; }
        public int ManagerId { get; set; }
        public List<EmployeeNode> Subordinates { get; set; } = new List<EmployeeNode>();
    }
}
