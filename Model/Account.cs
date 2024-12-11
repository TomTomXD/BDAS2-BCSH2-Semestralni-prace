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

        // Nevytváříme objekt AccountType v databázi jsou hodnoty omezeny jen na B (běžný) a S (spořicí)),
        // v databázi jako supertyp a subtypy
        public string FriendlyAccountType
        {
            get
            {
                return AccountType switch
                {
                    "B" => "Běžný",
                    "S" => "Spořicí",
                    _ => "Neznámý"
                };
            }
        }
    }
}
