using FinancniInformacniSystemBanky.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class LogsService
    {
        private readonly DatabaseService _databaseService;

        public LogsService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<Log> GetLogs()
        {
            string query = "SELECT * FROM Logy";

            return _databaseService.ExecuteSelect(query, reader => new Log
            {
                Log_Id = reader.GetInt32(0),
                Operation = reader.GetString(1),
                TimeStamp = reader.GetDateTime(2),
                AffectedTableName = reader.GetString(3),
                AffectedRecordId = reader.GetInt32(4),
                OldData = reader.IsDBNull(5) ? null : reader.GetString(5),
                NewData = reader.IsDBNull(6) ? null : reader.GetString(6),
                LogDescription = reader.GetString(7)
            });
        }
    }
}
