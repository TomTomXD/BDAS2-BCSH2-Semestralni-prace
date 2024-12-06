using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class LookupTablesService
    {
        private readonly DatabaseService _databaseService;

        public LookupTablesService()
        {
            _databaseService = new DatabaseService();
        }
    }
}
