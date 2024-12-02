using FinancniInformacniSystemBanky.Model;

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
            //    DateTime datumVystaveni,
            //    DateTime datumPlatnosti,
            //    string cvv,
            //    int idTypuKarty,
            //    int idBeznehoUctu,
            //    string vydavatel = "53"
            //    ) // Default value for vydavatel
            //{
            //    try
            //    {
            //        var procedureName = "upsert_karta"; // Name of the procedure

            //        // DatabaseService to execute the procedure
            //        DatabaseService dbService = new DatabaseService();
            //        dbService.ExecuteProcedure(procedureName, command =>
            //        {
            //            // Parameters for the procedure
            //            command.Parameters.Add("p_id_karty", OracleDbType.Int32).Value = DBNull.Value; // NULL for a new card
            //            command.Parameters.Add("p_datum_vydani", OracleDbType.Date).Value = datumVystaveni;
            //            command.Parameters.Add("p_datum_platnosti", OracleDbType.Date).Value = datumPlatnosti;
            //            command.Parameters.Add("p_cvv_kod", OracleDbType.Char).Value = cvv; // Default CVV code
            //            command.Parameters.Add("p_id_typu_karty", OracleDbType.Int32).Value = idTypuKarty;
            //            command.Parameters.Add("p_bezny_ucet_id_ucet", OracleDbType.Int32).Value = idBeznehoUctu;
            //            command.Parameters.Add("p_vydavatel", OracleDbType.Char).Value = vydavatel; // Pass the vydavatel (issuer) code
            //        });

            //        MessageBox.Show("Karta byla úspěšně přidána.", "Přidání karty", MessageBoxButton.OK, MessageBoxImage.Information);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Nepodařilo se přidat kartu. Chyba: {ex.Message}", "Přidání karty", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
        }



        public void RemoveCard()
        {
            throw new NotImplementedException();
        }

        public void EditCard()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CardType> GetCardTypes()
        {
            string query = @"SELECT * FROM TYPY_KARET";
            return _databaseService.ExecuteSelect(query, reader => new CardType
            {
                CardTypeId = reader.GetInt32(0),
                Type = reader.GetString(1)
            });
        }
    }
}