using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model.Helpers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FinancniInformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro EmployeeHierarchyView.xaml
    /// </summary>
    /// 

    // Není potřeba nějaká složitá logika, stačí pouze zobrazit stromovou strukturu zaměstnanců, nevytvářen žádný ViewModel
    public partial class EmployeeHierarchyView : UserControl
    {
        private readonly EmployeesService _employeesService;

        public EmployeeHierarchyView()
        {
            InitializeComponent();
            _employeesService = new EmployeesService();
            LoadEmployeeHierarchy();
        }

        private void LoadEmployeeHierarchy()
        {
            var employees = _employeesService.GetEmployeeHierarchy();
            if (employees == null || !employees.Any())
            {
                MessageBox.Show("No employees found.");
                return;
            }

            var rootElements = CreateTreeItems(employees.ToList());
            EmployeeTreeView.ItemsSource = rootElements;
        }

        private ObservableCollection<TreeViewItem> CreateTreeItems(List<EmployeeNode> employees)
        {
            var items = new ObservableCollection<TreeViewItem>();

            foreach (var employee in employees)
            {
                var treeViewItem = new TreeViewItem { Header = employee.FullName };
                treeViewItem.ItemsSource = CreateTreeItems(employee.Subordinates);
                items.Add(treeViewItem);
            }

            return items;
        }
    }
}