using FinancniInformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

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
        public IEnumerable<Role> GetRoles()
        {
            string query = "SELECT * FROM ROLE";
            return _databaseService.ExecuteSelect(query, reader =>
             {
               return new Role
                 {
                     Id = reader.GetInt32(0),
                     Name = reader.GetString(1)
                 };
             });
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
