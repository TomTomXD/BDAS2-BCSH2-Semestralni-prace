using FinancniInformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

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

        public void AddCard(
                string cisloKarty,
                DateTime datumVystaveni,
                DateTime datumPlatnosti,
                string cvv,
                int idTypuKarty,
                int idBeznehoUctu
                )
        // Default value for vydavatel
        {
            try
            {
                var procedureName = "upsert_karta"; // Name of the procedure

                // DatabaseService to execute the procedure
                DatabaseService dbService = new DatabaseService();
                dbService.ExecuteProcedure(procedureName, command =>
                {
                    // Parameters for the procedure
                    command.Parameters.Add("p_id_karty", OracleDbType.Int32).Value = DBNull.Value; // NULL for a new card
                    command.Parameters.Add("p_cislo_karty",OracleDbType.Char).Value = cisloKarty; // Generate card number (function call
                    command.Parameters.Add("p_datum_vydani", OracleDbType.Date).Value = datumVystaveni;
                    command.Parameters.Add("p_datum_platnosti", OracleDbType.Date).Value = datumPlatnosti;
                    command.Parameters.Add("p_cvv_kod", OracleDbType.Char).Value = cvv; // Default CVV code
                    command.Parameters.Add("p_id_typu_karty", OracleDbType.Int32).Value = idTypuKarty;
                    command.Parameters.Add("p_bezny_ucet_id_ucet", OracleDbType.Int32).Value = idBeznehoUctu;
                });

                MessageBox.Show("Karta byla úspěšně přidána.", "Přidání karty", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se přidat kartu. Chyba: {ex.Message}", "Přidání karty", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveCard(int cardId)
        {
            try
            {
                string query = "DELETE FROM KARTY WHERE ID_KARTY = :id_karta";

                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add("id_karta", OracleDbType.Int32).Value = cardId;
                });
                MessageBox.Show("Karta byla úspěšně odebrána", "Odebrání karty", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void UpdateCard(
                int idKarty,
                string cisloKarty,
                DateTime datumVystaveni,
                DateTime datumPlatnosti,
                string cvv,
                int idTypuKarty,
                int idBeznehoUctu)
        {
            try
            {
                var procedureName = "upsert_karta"; 

                DatabaseService dbService = new DatabaseService();
                dbService.ExecuteProcedure(procedureName, command =>
                {
                    command.Parameters.Add("p_id_karty", OracleDbType.Int32).Value = idKarty; 
                    command.Parameters.Add("p_cislo_karty", OracleDbType.Char).Value = cisloKarty; 
                    command.Parameters.Add("p_datum_vydani", OracleDbType.Date).Value = datumVystaveni;
                    command.Parameters.Add("p_datum_platnosti", OracleDbType.Date).Value = datumPlatnosti;
                    command.Parameters.Add("p_cvv_kod", OracleDbType.Char).Value = cvv; 
                    command.Parameters.Add("p_id_typu_karty", OracleDbType.Int32).Value = idTypuKarty;
                    command.Parameters.Add("p_bezny_ucet_id_ucet", OracleDbType.Int32).Value = idBeznehoUctu;
                });

                MessageBox.Show("Karta byla úspěšně upravena.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se upravit kartu. Chyba: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        public string GenerateCardNumber(int cisloVadavatele)
        {
            try
            {
                // Příprava SQL dotazu pro volání funkce
                string query = $"SELECT generovat_unikatni_cislo_karty({cisloVadavatele}) FROM dual";

                // Zavolání funkce a vrátí vygenerované číslo karty
                return _databaseService.ExecuteScalar(query) as string;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null; // nebo nějaké defaultní vrácení
            }
        }
    }
}