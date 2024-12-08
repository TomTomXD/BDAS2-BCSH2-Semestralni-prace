﻿using Microsoft.VisualBasic;
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
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int LicenceType { get; set; }
        public int LicenceHolderId { get; set; }

        public string FormattedIssueDate => IssueDate.ToString("dd.MM. yyyy");
        public string FormattedExpirationDate => ExpirationDate.ToString("dd.MM. yyyy");
    }
}
