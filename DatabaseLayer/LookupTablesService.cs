using FinancniInformacniSystemBanky.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class LookupTablesService
    {
        private readonly DatabaseService _databaseService;

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
            var storedProcedure =produre;

            try
            {
                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    command.Parameters.Add("p_id", DBNull.Value);
                    command.Parameters.Add("p_name", name);
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
