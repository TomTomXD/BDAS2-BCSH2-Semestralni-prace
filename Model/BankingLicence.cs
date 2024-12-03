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
        public string LicenceNumber { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int LicenceType { get; set; }
        public int LicenceHolderId { get; set; }
    }
}
