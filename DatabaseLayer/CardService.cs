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
            string query = @"SELECT * FROM KARTY";
            return _databaseService.ExecuteSelect(query, reader => new Card
            {
                CardId = reader.GetInt32(0),
                CardNumber = reader.GetString(1),
                IssuedDate = reader.GetDateTime(2),
                ExpirationDate = reader.GetDateTime(3),
                CVV = reader.GetString(4),
                CardType = reader.GetString(5),
                AccountId = reader.GetInt32(6)
            });
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
