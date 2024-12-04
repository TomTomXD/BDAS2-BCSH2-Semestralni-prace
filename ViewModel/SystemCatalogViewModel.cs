using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using System.Collections.ObjectModel;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class SystemCatalogViewModel
    {
        private readonly SystemCatalogService _systemCatalogService;

        public ObservableCollection<SystemCatalog> SystemCatalog { get; set; }
        public SystemCatalogViewModel()
        {
            _systemCatalogService = new SystemCatalogService();
            SystemCatalog = new ObservableCollection<SystemCatalog>();

            LoadSystemCatalogFromDatabase();
        }

        private void LoadSystemCatalogFromDatabase()
        {
            var systemCatalogFromDb = _systemCatalogService.GetSystemCatalog();
            SystemCatalog.Clear();
            foreach (var systemCatalog in systemCatalogFromDb)
            {
                SystemCatalog.Add(systemCatalog);
            }
        }
    }
}
