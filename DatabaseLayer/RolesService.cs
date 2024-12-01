using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class RolesService
    {
        private readonly DatabaseService _databaseService;

        public RolesService()
        {
            _databaseService = new DatabaseService();
        }

        /// <summary>
        /// Načte seznam rolí z databáze jako IEnumerable.
        /// </summary>
        /// <returns>IEnumerable obsahující názvy rolí.</returns>
        public IEnumerable<string> GetRoles()
        {
            const string query = "SELECT role FROM ROLE";

            foreach (var role in _databaseService.ExecuteSelect(
                query,
                reader => reader["role"].ToString()
            ))
            {
                yield return role; // Vracení hodnot pomocí `yield return`.
            }
        }

        /// <summary>
        /// Vrátí ID role podle názvu role.
        /// </summary>
        /// <param name="roleName">Název role.</param>
        /// <returns>Vrací ID role, nebo -1, pokud role neexistuje.</returns>
        public int GetRoleId(string roleName)
        {
            const string query = "SELECT ID_ROLE FROM ROLE WHERE role = :roleName";

            try
            {
                object result = _databaseService.ExecuteScalar(
                    query,
                    command => command.Parameters.Add(new OracleParameter(":roleName", roleName))
                );

                // Pokud výsledek je null, role neexistuje
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání ID role: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // Vrátíme -1 v případě chyby
            }
        }

    }
}
