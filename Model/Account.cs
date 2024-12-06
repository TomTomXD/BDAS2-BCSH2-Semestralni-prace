using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformacniSystemBanky.Model
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal PaymentLimit { get; set; }
        public int PersonId { get; set; }
        public string OwnerName { get; set; }
        public string AccountType { get; set; }
    }

}
