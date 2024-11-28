using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class UserService
    {
        private readonly DatabaseService _databaseService;

        public UserService()
        {
            _databaseService = new DatabaseService();
        }

        public void RegisterNewUser(
            string jmeno,
            string prijmeni,
            DateTime datumNarozeni,
            string rodneCislo,
            string telefon,
            string email,
            string typOsoby,
            int idRole,
            string ulice,
            string cisloPopisne,
            string mesto,
            int psc,
            string hash,
            string salt,
            string oddeleni = null,
            string pozice = null)
        {
            _databaseService.ExecuteProcedure("insert_osoba_adresa_heslo", command =>
            {
                command.Parameters.Add(new OracleParameter("p_jmeno", jmeno));
                command.Parameters.Add(new OracleParameter("p_prijmeni", prijmeni));
                command.Parameters.Add(new OracleParameter("p_datum_narozeni", datumNarozeni));
                command.Parameters.Add(new OracleParameter("p_rodne_cislo", rodneCislo));
                command.Parameters.Add(new OracleParameter("p_telefon", telefon));
                command.Parameters.Add(new OracleParameter("p_email", email));
                command.Parameters.Add(new OracleParameter("p_typ_osoby", typOsoby));
                command.Parameters.Add(new OracleParameter("p_id_role", idRole));
                command.Parameters.Add(new OracleParameter("p_ulice", ulice));
                command.Parameters.Add(new OracleParameter("p_cislo_popisne", cisloPopisne));
                command.Parameters.Add(new OracleParameter("p_mesto", mesto));
                command.Parameters.Add(new OracleParameter("p_psc", psc));
                command.Parameters.Add(new OracleParameter("p_hash", hash));
                command.Parameters.Add(new OracleParameter("p_salt", salt));
                command.Parameters.Add(new OracleParameter("p_oddeleni", oddeleni ?? (object)DBNull.Value));
                command.Parameters.Add(new OracleParameter("p_pozice", pozice ?? (object)DBNull.Value));
            });
        }

        public bool LoginUser(string email, string heslo)
        {
            try
            {
                var result = _databaseService.ExecuteSelect("SELECT h.hash, h.salt " +
                                                            "FROM HESLA h " +
                                                            "JOIN OSOBY o ON h.id_osoba = o.id_osoba " +
                                                            "WHERE o.email = :p_email", reader =>
                                                            {
                                                                return new
                                                                {
                                                                    Hash = reader.GetString(0),
                                                                    Salt = reader.GetString(1)
                                                                };
                                                            }, command =>
                                                            {
                                                                command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2)
                                                                {
                                                                    Value = email
                                                                });
                                                            });

                // Kontrolujte, zda byly nalezeny nějaké výsledky
                if (result.Count == 0)
                {
                    MessageBox.Show("Uživatel nenalezen.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false; // Uživatel nenalezen
                }

                var user = result.First();

                // Ověření hesla
                if (PasswordHasher.VerifyPassword(heslo, user.Hash))
                {
                    MessageBox.Show("Uživatel přihlášen.", "Přihlášení", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true; // Přihlášení úspěšné
                }
                else
                {
                    MessageBox.Show("Nesprávné heslo.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false; // Špatné heslo
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba při vykonávání dotazu: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočekávaná chyba: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false; // Vrátí false, pokud došlo k chybě
        }



    }

}
