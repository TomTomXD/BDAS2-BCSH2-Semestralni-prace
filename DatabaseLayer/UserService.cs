﻿using FinancniInformacniSystemBanky.Model;
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
            int psc,
            string hash,
            string salt,
            int? id = null,
            string? oddeleni = null,
            string? pozice = null)
        {
            try
            {
                _databaseService.ExecuteProcedure("upsert_osoba_adresa_heslo", command =>
                {
                    command.Parameters.Add(new OracleParameter("p_id_osoba", OracleDbType.Int32) { Value = id });
                    command.Parameters.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2) { Value = jmeno });
                    command.Parameters.Add(new OracleParameter("p_prijmeni", OracleDbType.Varchar2) { Value = prijmeni });
                    command.Parameters.Add(new OracleParameter("p_datum_narozeni", OracleDbType.Date) { Value = datumNarozeni });
                    command.Parameters.Add(new OracleParameter("p_rodne_cislo", OracleDbType.Varchar2) { Value = rodneCislo });
                    command.Parameters.Add(new OracleParameter("p_telefon", OracleDbType.Char) { Value = telefon });
                    command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2) { Value = email });
                    command.Parameters.Add(new OracleParameter("p_typ_osoby", OracleDbType.Char) { Value = typOsoby });
                    command.Parameters.Add(new OracleParameter("p_id_role", OracleDbType.Int32) { Value = idRole });
                    command.Parameters.Add(new OracleParameter("p_ulice", OracleDbType.Varchar2) { Value = ulice });
                    command.Parameters.Add(new OracleParameter("p_cislo_popisne", OracleDbType.Char) { Value = cisloPopisne });
                    command.Parameters.Add(new OracleParameter("p_mesto", OracleDbType.Varchar2) { Value = mesto });
                    command.Parameters.Add(new OracleParameter("p_psc", OracleDbType.Int32) { Value = psc });
                    command.Parameters.Add(new OracleParameter("p_hash", OracleDbType.Varchar2) { Value = hash });
                    command.Parameters.Add(new OracleParameter("p_salt", OracleDbType.Varchar2) { Value = salt });
                    command.Parameters.Add(new OracleParameter("p_oddeleni", OracleDbType.Varchar2) { Value = oddeleni ?? (object)DBNull.Value });
                    command.Parameters.Add(new OracleParameter("p_pozice", OracleDbType.Varchar2) { Value = pozice ?? (object)DBNull.Value });
                });

                return true; // Vložení úspěšné
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba při vykonávání procedury: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočekávaná chyba: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false; // Vložení neúspěšné
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
    string ulice,
    string cisloPopisne,
    string mesto,
    int psc,
    string? oddeleni = null,
    string? pozice = null)
        {
            try
            {
                _databaseService.ExecuteProcedure("upsert_osoba_adresa_heslo", command =>
                {
                    command.Parameters.Add(new OracleParameter("p_id_osoba", OracleDbType.Int32) { Value = id });
                    command.Parameters.Add(new OracleParameter("p_jmeno", OracleDbType.Varchar2) { Value = jmeno });
                    command.Parameters.Add(new OracleParameter("p_prijmeni", OracleDbType.Varchar2) { Value = prijmeni });
                    command.Parameters.Add(new OracleParameter("p_datum_narozeni", OracleDbType.Date) { Value = datumNarozeni });
                    command.Parameters.Add(new OracleParameter("p_rodne_cislo", OracleDbType.Varchar2) { Value = rodneCislo });
                    command.Parameters.Add(new OracleParameter("p_telefon", OracleDbType.Char) { Value = telefon });
                    command.Parameters.Add(new OracleParameter("p_email", OracleDbType.Varchar2) { Value = email });
                    command.Parameters.Add(new OracleParameter("p_ulice", OracleDbType.Varchar2) { Value = ulice });
                    command.Parameters.Add(new OracleParameter("p_cislo_popisne", OracleDbType.Char) { Value = cisloPopisne });
                    command.Parameters.Add(new OracleParameter("p_mesto", OracleDbType.Varchar2) { Value = mesto });
                    command.Parameters.Add(new OracleParameter("p_psc", OracleDbType.Int32) { Value = psc });
                    command.Parameters.Add(new OracleParameter("p_oddeleni", OracleDbType.Varchar2) { Value = oddeleni ?? (object)DBNull.Value });
                    command.Parameters.Add(new OracleParameter("p_pozice", OracleDbType.Varchar2) { Value = pozice ?? (object)DBNull.Value });
                    command.Parameters.Add(new OracleParameter("p_hash", OracleDbType.Varchar2) { Value = DBNull.Value }); // For edits, password-related fields can be ignored
                    command.Parameters.Add(new OracleParameter("p_salt", OracleDbType.Varchar2) { Value = DBNull.Value });
                });

                MessageBox.Show("Uživatel úspěšně upraven.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Chyba při vykonávání procedury: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočekávaná chyba: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }

    }
}
