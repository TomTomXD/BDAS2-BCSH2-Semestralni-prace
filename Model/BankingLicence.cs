using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformacniSystemBanky.Model
{
    public class BankingLicence
    {
        public int BankingLicenseId { get; set; }
        public int LicenseNumber { get; set; }
        public DateFormat IssueDate { get; set; }
        public DateFormat ExpirationDate { get; set; }
    }
}
