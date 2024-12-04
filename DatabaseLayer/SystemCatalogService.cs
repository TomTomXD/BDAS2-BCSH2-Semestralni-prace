using FinancniInformacniSystemBanky.Model;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class SystemCatalogService
    {
        private readonly DatabaseService _databaseService;

        public SystemCatalogService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<SystemCatalog> GetSystemCatalog()
        {
            string query = @"
                       SELECT 'TABLE' AS OBJECT_TYPE, TABLE_NAME AS OBJECT_NAME
                FROM USER_TABLES
                UNION ALL
                SELECT 'VIEW' AS OBJECT_TYPE, VIEW_NAME AS OBJECT_NAME
                FROM USER_VIEWS
                UNION ALL
                SELECT 'PROCEDURE' AS OBJECT_TYPE, OBJECT_NAME AS OBJECT_NAME
                FROM USER_PROCEDURES
                WHERE OBJECT_TYPE = 'PROCEDURE'
                UNION ALL
                SELECT 'FUNCTION' AS OBJECT_TYPE, OBJECT_NAME AS OBJECT_NAME
                FROM USER_PROCEDURES
                WHERE OBJECT_TYPE = 'FUNCTION'
                UNION ALL
                SELECT 'TRIGGER' AS OBJECT_TYPE, TRIGGER_NAME AS OBJECT_NAME
                FROM USER_TRIGGERS
                UNION ALL
                SELECT 'SEQUENCE' AS OBJECT_TYPE, SEQUENCE_NAME AS OBJECT_NAME
                FROM USER_SEQUENCES
                UNION ALL
                SELECT 'INDEX' AS OBJECT_TYPE, INDEX_NAME AS OBJECT_NAME
                FROM USER_INDEXES";

            return _databaseService.ExecuteSelect(query, reader => new SystemCatalog
            {
                ObjectType = reader.GetString(0),
                ObjectName = reader.GetString(1)
            });
        }

    }
}
