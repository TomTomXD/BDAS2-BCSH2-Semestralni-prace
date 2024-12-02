using FinancniInformacniSystemBanky.DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class BankingLicencesViewModel
    {
        private readonly BankingLicenceService _bankingLicencesService;

        public BankingLicencesViewModel()
        {
            _bankingLicencesService = new BankingLicenceService();
        }
    }
}
