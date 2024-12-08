using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model
{
    public class Log
    {
        public int Log_Id { get; set; }
        public string Operation { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AffectedTableName { get; set; }
        public int AffectedRecordId { get; set; }
        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public string LogDescription { get; set; }
    }
}
