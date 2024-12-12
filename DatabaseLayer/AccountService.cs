using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class AccountService
    {
        private readonly DatabaseService _databaseService;

        public AccountService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<Account> GetAccounts(int? id = null)
        {
            string query;
            Action<OracleCommand> configureCommand = null;

            if (id != null)
            {
                query = "SELECT * FROM UCTY_VIEW WHERE KLIENT_ID_OSOBA = :id";
                configureCommand = command =>
                {
                    command.Parameters.Add(new OracleParameter("id", id.Value));
                };
            }
            else
            {
                query = "SELECT * FROM UCTY_VIEW";
            }

            return _databaseService.ExecuteSelect(query, reader => new Account
            {
                AccountId = reader.GetInt32(0),
                AccountNumber = reader.GetString(1),
                Balance = reader.GetDecimal(2),
                PaymentLimit = reader.GetDecimal(3),
                PersonId = reader.GetInt32(4),
                OwnerName = reader.GetString(5),
                AccountType = reader.GetString(6)
            }, configureCommand);
        }

        public IEnumerable<Account> GetAccountsById(int id)
        {
            {
                string query = "SELECT * FROM UCTY_VIEW WHERE KLIENT_ID_OSOBA =: id";

                return _databaseService.ExecuteSelect(query, reader => new Account
                {
                    AccountId = reader.GetInt32(0),
                    AccountNumber = reader.GetString(1),
                    Balance = reader.GetDecimal(2),
                    PaymentLimit = reader.GetDecimal(3),
                    PersonId = reader.GetInt32(4),
                    OwnerName = reader.GetString(5),
                    AccountType = reader.GetString(6)
                }, command =>
                {
                    command.Parameters.Add(new OracleParameter("id", id));
                });
            }
        }

        public IEnumerable<Account> GetAccountsByIdAndType(int id, char type)
        {
            string query = "SELECT * FROM UCTY_VIEW WHERE KLIENT_ID_OSOBA = :id AND TYP_UCTU = :type";

            return _databaseService.ExecuteSelect(query, reader => new Account
            {
                AccountId = reader.GetInt32(0),
                AccountNumber = reader.GetString(1),
                Balance = reader.GetDecimal(2),
                PaymentLimit = reader.GetDecimal(3),
                PersonId = reader.GetInt32(4),
                OwnerName = reader.GetString(5),
                AccountType = reader.GetString(6)
            }, command =>
            {
                command.Parameters.Add(new OracleParameter("id", id));
                command.Parameters.Add(new OracleParameter("type", type)); // Přidání parametru pro typ účtu
            });
        }

        public void AddAccount(
            string cisloUctu,
            decimal zustatek,
            decimal limitProPlatby,
            int klientIdOsoba,
            char typUctu,
            decimal? rocniUrokovaSazba = null,
            decimal? maximalniZustatek = null)
        {
            try
            {
                var procedureName = "upsert_ucet"; // Název procedury

                // Zavolejte proceduru s parametry
                DatabaseService dbService = new DatabaseService();
                dbService.ExecuteProcedure(procedureName, command =>
                {
                    // Nastavení parametrů procedury
                    command.Parameters.Add("p_id_ucet", OracleDbType.Int32).Value = DBNull.Value; // NULL pro nový účet
                    command.Parameters.Add("p_cislo_uctu", OracleDbType.Char).Value = cisloUctu;
                    command.Parameters.Add("p_zustatek", OracleDbType.Decimal).Value = zustatek;
                    command.Parameters.Add("p_limit_pro_platby", OracleDbType.Decimal).Value = limitProPlatby;
                    command.Parameters.Add("p_klient_id_osoba", OracleDbType.Int32).Value = klientIdOsoba;
                    command.Parameters.Add("p_typ_uctu", OracleDbType.Char).Value = typUctu;

                    // Parametry specifické pro spořicí účet
                    if (rocniUrokovaSazba.HasValue)
                    {
                        command.Parameters.Add("p_rocni_urokova_sazba", OracleDbType.Decimal).Value = rocniUrokovaSazba.Value;
                    }
                    else
                    {
                        command.Parameters.Add("p_rocni_urokova_sazba", OracleDbType.Decimal).Value = DBNull.Value;
                    }

                    if (maximalniZustatek.HasValue)
                    {
                        command.Parameters.Add("p_maximalni_zustatek", OracleDbType.Decimal).Value = maximalniZustatek.Value;
                    }
                    else
                    {
                        command.Parameters.Add("p_maximalni_zustatek", OracleDbType.Decimal).Value = DBNull.Value;
                    }
                });

                MessageBox.Show("Účet byl úspěšně přidán.", "Přidání účtu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se přidat účet. Chyba: {ex.Message}", "Přidání účtu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void DeleteAccount(string accountNumber)
        {
            try
            {
                string query = @"DELETE FROM UCTY WHERE CISLO_UCTU = :accountNumber";
                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add(new OracleParameter("accountNumber", accountNumber));
                });
                MessageBox.Show("Účet odstraněn.", "Odstranění účtu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nepodařilo se odstranit účet.", "Odstranění účtu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateAccount(
            int idUctu,
            string cisloUctu,
            decimal zustatek,
            decimal limitProPlatby,
            int klientIdOsoba,
            char typUctu,
            decimal? rocniUrokovaSazba = null,
            decimal? maximalniZustatek = null)
        {
            try
            {
                var procedureName = "upsert_ucet"; // Název procedury

                // Zavolejte proceduru s parametry
                DatabaseService dbService = new DatabaseService();
                dbService.ExecuteProcedure(procedureName, command =>
                {
                    // Nastavení parametrů procedury
                    command.Parameters.Add("p_id_ucet", OracleDbType.Int32).Value = idUctu; // NULL pro nový účet
                    command.Parameters.Add("p_cislo_uctu", OracleDbType.Char).Value = cisloUctu;
                    command.Parameters.Add("p_zustatek", OracleDbType.Decimal).Value = zustatek;
                    command.Parameters.Add("p_limit_pro_platby", OracleDbType.Decimal).Value = limitProPlatby;
                    command.Parameters.Add("p_klient_id_osoba", OracleDbType.Int32).Value = klientIdOsoba;
                    command.Parameters.Add("p_typ_uctu", OracleDbType.Char).Value = typUctu;

                    // Parametry specifické pro spořicí účet
                    if (rocniUrokovaSazba.HasValue)
                    {
                        command.Parameters.Add("p_rocni_urokova_sazba", OracleDbType.Decimal).Value = rocniUrokovaSazba.Value;
                    }
                    else
                    {
                        command.Parameters.Add("p_rocni_urokova_sazba", OracleDbType.Decimal).Value = DBNull.Value;
                    }

                    if (maximalniZustatek.HasValue)
                    {
                        command.Parameters.Add("p_maximalni_zustatek", OracleDbType.Decimal).Value = maximalniZustatek.Value;
                    }
                    else
                    {
                        command.Parameters.Add("p_maximalni_zustatek", OracleDbType.Decimal).Value = DBNull.Value;
                    }
                });

                MessageBox.Show("Účet byl úspěšně přidán.", "Přidání účtu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se přidat účet. Chyba: {ex.Message}", "Přidání účtu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public IEnumerable<Client> GetPossibleOwners()
        {
            string query = "SELECT * FROM VSICHNI_KLIENTI_VIEW";
            return _databaseService.ExecuteSelect(query, reader => new Client
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                NationalIdNumber = reader.GetString(3)
            });
        }

        public void ChangeLimit(int accountId, decimal newLimit)
        {
            try
            {
                string query = "UPDATE UCTY SET LIMIT_PRO_PLATBY = :newLimit WHERE ID_UCET = :accountId";

                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add(new OracleParameter("newLimit", newLimit));
                    command.Parameters.Add(new OracleParameter("accountId", accountId));
                });
                MessageBox.Show("Limit byl úspěšně změněn.", "Změna limitu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se změnit limit. Chyba: {ex.Message}", "Změna limitu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddClientAccount(int clientId)
        {
            string storedProcedure = "insert_bezny_ucet";

            try
            {
                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    command.Parameters.Add("p_klient_id_osoba", OracleDbType.Int32).Value = clientId;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se přidat účet. Chyba: {ex.Message}", "Přidání účtu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LockedAccount(string accountNumber)
        {
            string storedProcedure = "zablokuj_bezny_ucet";
            try
            {
                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    command.Parameters.Add("p_cislo_uctu", OracleDbType.Char).Value = accountNumber;
                });
                MessageBox.Show("Účet byl úspěšně zablokován.\n" + "Kontaktuje nebo navštivte naší nejbližší pobočku ohledně dalšího postupu.", "Zablokování účtu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepodařilo se zablokovat účet. Chyba: {ex.Message}", "Zablokování účtu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}