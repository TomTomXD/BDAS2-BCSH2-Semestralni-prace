using FinancniInformacniSystemBanky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class PasswordService
    {
        private readonly DatabaseService _databaseService;

        public PasswordService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<Password> GetPasswords()
        {
                string query = "SELECT * FROM HESLA";

                return _databaseService.ExecuteSelect(query, reader => new Password
                {
                    PasswordId = reader.GetInt32(0),
                    HashedPassword = reader.GetString(1),
                    Salt = reader.GetString(2),
                    UserId = reader.GetInt32(3)
                });
        }

        public void UpdatePassword(int selectedPassword, string newPasswordHash, string newPasswordSalt)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeletePassword(int passwordId)
        {
            try
            {
                string query = "DELETE FROM HESLA WHERE id_heslo = :idPassword";

                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add("idPassword", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = passwordId;
                });
                MessageBox.Show("Heslo bylo úspěšně smazáno", "Smazání hesla", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
