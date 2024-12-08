using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class LogsViewModel
    {
        private readonly LogsService _logsService;
        public ObservableCollection<Log> Logs { get; set; }


        public LogsViewModel()
        {
            _logsService = new LogsService();
            Logs = new ObservableCollection<Log>(_logsService.GetLogs());
        }

    }
}
