using FinancniInformacniSystemBanky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class CardService
    {
        private readonly DatabaseService _databaseService;

        public CardService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<Card> GetCards()
        {
            throw new NotImplementedException();
        }

        public void AddCard()
        {
            throw new NotImplementedException();
        }

        public void RemoveCard()
        {
            throw new NotImplementedException();
        }

        public void EditCard()
        {
            throw new NotImplementedException();
        }
    }
}
