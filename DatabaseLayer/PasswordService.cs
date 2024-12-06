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
                string query = @"SELECT h.id_heslo, 
                                    o.email, 
                                    h.hash, 
                                    h.salt
                                    FROM HESLA h
                                    JOIN OSOBY o ON h.id_osoba = o.id_osoba";

                return _databaseService.ExecuteSelect(query, reader => new Password
                {
                    PasswordId = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    HashedPassword = reader.GetString(2),
                    Salt = reader.GetString(3),
                });
        }

        public void UpdatePassword(int selectedPasswordId, string newPasswordHash, string newPasswordSalt)
        {
            try
            {
                string query = "UPDATE HESLA SET hash = :newPasswordHash, salt = :newPasswordSalt WHERE id_heslo = :idPassword";
                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add("newPasswordHash", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = newPasswordHash;
                    command.Parameters.Add("newPasswordSalt", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = newPasswordSalt;
                    command.Parameters.Add("idPassword", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = selectedPasswordId;
                });
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
