using FinancniInformacniSystemBanky.Model.Helpers;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class LookupTablesService
    {
        private readonly DatabaseService _databaseService;
        private static readonly HashSet<string> AllowedTables = new HashSet<string>
        {
            "ODDELENI",
            "POZICE",
            "ROLE",
            "STAVY_UVERU",
            "TYPY_LICENCI",
            "TYPY_UVERU",
            "TYPY_KARET"
        };

        private static readonly HashSet<string> AllowedColumns = new HashSet<string>
        {
            "ID_ODDELENI",
            "ID_POZICE",
            "ID_ROLE",
            "ID_STAVU_UVERU",
            "ID_TYPU_LICENCE",
            "ID_TYPU_UVERU",
            "ID_TYPU_KARTY"
        };

        public LookupTablesService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<E> GetLookupTableData<E>(string table) where E : ILookupEntry, new()
        {
            string query = $"SELECT * FROM {table}";

            return _databaseService.ExecuteSelect(query, reader =>
            {
                return new E
                {
                    Id = reader.GetInt32(0),  // První sloupec jako INT
                    Name = reader.GetString(1)  // Druhý sloupec jako STRING
                };
            });
        }

        public void InsertIntoLookupTable(string produre, string name)
        {
            var storedProcedure = produre;

            try
            {
                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    command.Parameters.Add("p_id", DBNull.Value);
                    command.Parameters.Add("p_name", name);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateLookupTable(string procedure, int id, string name)
        {
            var storedProcedure = procedure;

            try
            {
                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    command.Parameters.Add("p_id", id);
                    command.Parameters.Add("p_name", name);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // předáváme condtion jako string, jelikož v každé tabulce je sloupec jinak pojmenovaný,
        // povolené hodnoty jsou ošetřeny pomocí HashSetu, aby nedošlo k SQL Injection
        public void DeleteRecord(string table, string id_column, int id)
        {
            if (!AllowedTables.Contains(table))
            {
                throw new ArgumentException("Neplatný název tabulky.", nameof(table));
            }

            if (!AllowedColumns.Contains(id_column))
            {
                throw new ArgumentException("Neplatný název sloupce.", nameof(id_column));
            }

            string query = $"DELETE FROM {table} WHERE {id_column} = :id";
            try
            {
                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add("id", id);
                });
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 2292) // ORA-02292
            {
                MessageBox.Show(
                    "Nelze odstranit záznam, protože existují podřízené záznamy v jiné tabulce.",
                    "Chyba mazání",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}