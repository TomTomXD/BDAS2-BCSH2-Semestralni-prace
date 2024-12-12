using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model.Helpers
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? NationalIdNumber { get; set; }
        public string FullDesc => $"{FirstName} {LastName} (r.č.: {NationalIdNumber})";
        public string FullName => $"{FirstName} {LastName}";
    }
}
