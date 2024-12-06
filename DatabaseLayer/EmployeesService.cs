using FinancniInformacniSystemBanky.Model.Helpers;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class EmployeesService
    {
        private readonly DatabaseService _databaseService;

        public EmployeesService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<EmployeeNode> GetEmployeeHierarchy()
        {
            string query = @"
                SELECT 
                    o.ID_OSOBA,
                    o.JMENO,
                    o.PRIJMENI,
                    oD.ODDELENI,
                    p.POZICE,
                    z.ZAMESTNANEC_ID_OSOBA
                FROM 
                    ZAMESTNANCI z
                JOIN 
                    OSOBY o ON z.ID_OSOBA = o.ID_OSOBA
                JOIN 
                    ODDELENI oD ON z.ID_ODDELENI = oD.ID_ODDELENI
                JOIN 
                    POZICE p ON z.ID_POZICE = p.ID_POZICE
                START WITH 
                    z.ZAMESTNANEC_ID_OSOBA IS NULL
                CONNECT BY 
                    PRIOR z.ID_OSOBA = z.ZAMESTNANEC_ID_OSOBA
                ORDER SIBLINGS BY 
                    z.ID_OSOBA
            ";

            var employees = _databaseService.ExecuteSelect(query, reader =>
            {
                return new EmployeeNode
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Department = reader.GetString(3),
                    Position = reader.GetString(4),
                    ManagerId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                };
            });

            var employeeMap = employees.ToDictionary(e => e.Id);

            foreach (var employee in employees)
            {
                if (employee.ManagerId != 0 && employeeMap.ContainsKey(employee.ManagerId))
                {
                    employeeMap[employee.ManagerId].Subordinates.Add(employee);
                }
            }

            return employees.Where(e => e.ManagerId == 0).ToList();
        }
    }
}