﻿using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class UserService
    {
        private readonly DatabaseService _databaseService;
        private readonly PersonService _personalDetails;

        public UserService()
        {
            _databaseService = new DatabaseService();
            _personalDetails = new PersonService();
        }

        public bool RegisterNewUser(
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
     string psc,
     string hash,
     string salt,
     int? oddeleni = null,
     int? pozice = null,
     int? idManager = null)
        {
            try
            {
                _databaseService.ExecuteProcedure("upsert_osoba_adresa_heslo", command =>
                {
                    command.Parameters.Add(new OracleParameter("p_id_osoba", OracleDbType.Int32)).Value = DBNull.Value; // pro nového zaměstnance
                    command.Parameters.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2)).Value = jmeno;
                    command.Parameters.Add(new OracleParameter("p_prijmeni", OracleDbType.Varchar2)).Value = prijmeni;
                    command.Parameters.Add(new OracleParameter("p_datum_narozeni", OracleDbType.Date)).Value = datumNarozeni;
                    command.Parameters.Add(new OracleParameter("p_rodne_cislo", OracleDbType.Varchar2)).Value = rodneCislo;
                    command.Parameters.Add(new OracleParameter("p_telefon", OracleDbType.Char)).Value = telefon;
                    command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2)).Value = email;
                    command.Parameters.Add(new OracleParameter("p_typ_osoby", OracleDbType.Char)).Value = typOsoby;
                    command.Parameters.Add(new OracleParameter("p_id_role", OracleDbType.Int32)).Value = idRole;
                    command.Parameters.Add(new OracleParameter("p_ulice", OracleDbType.Varchar2)).Value = ulice;
                    command.Parameters.Add(new OracleParameter("p_cislo_popisne", OracleDbType.Char)).Value = cisloPopisne;
                    command.Parameters.Add(new OracleParameter("p_mesto", OracleDbType.Varchar2)).Value = mesto;
                    command.Parameters.Add(new OracleParameter("p_psc", OracleDbType.Char)).Value = psc;
                    command.Parameters.Add(new OracleParameter("p_hash", OracleDbType.Varchar2)).Value = hash;
                    command.Parameters.Add(new OracleParameter("p_salt", OracleDbType.Varchar2)).Value = salt;
                    command.Parameters.Add(new OracleParameter("p_id_oddeleni", OracleDbType.Int32)).Value = oddeleni ?? (object)DBNull.Value;
                    command.Parameters.Add(new OracleParameter("p_id_pozice", OracleDbType.Int32)).Value = pozice ?? (object)DBNull.Value;
                    command.Parameters.Add(new OracleParameter("p_zamestnanec_id_osoba", OracleDbType.Int32)).Value = idManager ?? (object)DBNull.Value; // Ujistěte se, jestli ve vaší proceduře je tento parametr správně pojmenován
                });

                return true; // Úspěch
            }
            catch (OracleException ex)
            {
                if(ex.Number == 20001)
                {
                    MessageBox.Show("Aby se uživatel mohl zaregistrovat, musí mu být alespoň 10 let.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"Chyba při vykonávání procedury: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return false; // Neúspěch
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočekávaná chyba: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false; // Neúspěch
            }
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
                    // Získání ID uživatele a role
                    var userInfo = _databaseService.ExecuteSelect("SELECT o.id_osoba, o.id_role " +
                                                                  "FROM OSOBY o " +
                                                                  "WHERE o.email = :p_email", reader =>
                                                                  {
                                                                      return new
                                                                      {
                                                                          UserId = reader.GetInt32(0),
                                                                          RoleId = reader.GetInt32(1)
                                                                      };
                                                                  }, command =>
                                                                  {
                                                                      command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2)
                                                                      {
                                                                          Value = email
                                                                      });
                                                                  }).First();

                    // Nastavení přihlášeného uživatele v Session
                    Session.Instance.SetUser(userInfo.UserId, userInfo.RoleId);

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

        public bool EditUserDetails(
    int id,
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
    string psc,
    string? hash = null,
    string? salt = null,
    int? oddeleni = null,
    int? pozice = null,
    int? idManager = null)
        {
            try
            {
                _databaseService.ExecuteProcedure("upsert_osoba_adresa_heslo", command =>
                {
                    command.Parameters.Add(new OracleParameter("p_id_osoba", OracleDbType.Int32)).Value = id;
                    command.Parameters.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2)).Value = jmeno;
                    command.Parameters.Add(new OracleParameter("p_prijmeni", OracleDbType.Varchar2)).Value = prijmeni;
                    command.Parameters.Add(new OracleParameter("p_datum_narozeni", OracleDbType.Date)).Value = datumNarozeni;
                    command.Parameters.Add(new OracleParameter("p_rodne_cislo", OracleDbType.Varchar2)).Value = rodneCislo;
                    command.Parameters.Add(new OracleParameter("p_telefon", OracleDbType.Char)).Value = telefon;
                    command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2)).Value = email;
                    command.Parameters.Add(new OracleParameter("p_typ_osoby", OracleDbType.Char)).Value = typOsoby;
                    command.Parameters.Add(new OracleParameter("p_id_role", OracleDbType.Int32)).Value = idRole;
                    command.Parameters.Add(new OracleParameter("p_ulice", OracleDbType.Varchar2)).Value = ulice;
                    command.Parameters.Add(new OracleParameter("p_cislo_popisne", OracleDbType.Char)).Value = cisloPopisne;
                    command.Parameters.Add(new OracleParameter("p_mesto", OracleDbType.Varchar2)).Value = mesto;
                    command.Parameters.Add(new OracleParameter("p_psc", OracleDbType.Char)).Value = psc;
                    command.Parameters.Add(new OracleParameter("p_hash", OracleDbType.Varchar2)).Value = hash ?? (object)DBNull.Value;
                    command.Parameters.Add(new OracleParameter("p_salt", OracleDbType.Varchar2)).Value = salt ?? (object)DBNull.Value;
                    command.Parameters.Add(new OracleParameter("p_id_oddeleni", OracleDbType.Int32)).Value = oddeleni ?? (object)DBNull.Value;
                    command.Parameters.Add(new OracleParameter("p_id_pozice", OracleDbType.Int32)).Value = pozice ?? (object)DBNull.Value;
                    command.Parameters.Add(new OracleParameter("p_id_manager", OracleDbType.Int32)).Value = idManager ?? (object)DBNull.Value;
                });

                return true; // Úspěch
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočekávaná chyba: {ex.Message}",
                               "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false; // Neúspěch
            }
        }

        public bool DeleteUser(string nationalIdNumber)
        {
            try
            {
                // Spustíme DELETE SQL příkaz
                int rowsAffected = _databaseService.ExecuteNonQuery("DELETE FROM OSOBY WHERE RODNE_CISLO = :nationaIdNumber", command =>
                {
                    command.Parameters.Add(new OracleParameter(":nationaIdNumber", OracleDbType.Varchar2) { Value = nationalIdNumber });
                });

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Osoba s rodným číslem byla úspěšně smazána. Počet smazaných řádků: {rowsAffected}",
                                    "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba při mazání osoby: {ex.Message}",
                                "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočekávaná chyba: {ex.Message}",
                                "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }
    }
}
