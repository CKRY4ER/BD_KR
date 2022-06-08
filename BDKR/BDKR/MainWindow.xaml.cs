using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDKR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BuyerGrid.ItemsSource = BDKREntities.GetContext().Buyer.ToList();
            EmlpoyeeDataGrid.ItemsSource = BDKREntities.GetContext().Employee.ToList();
            SypplyDataGrid.ItemsSource = BDKREntities.GetContext().Sypply.ToList();
            OrderDataGrid.ItemsSource = BDKREntities.GetContext().OrderBuyer.ToList();
        }

        private void BuyerGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Buyer buyer = BuyerGrid.SelectedItem as Buyer;
            new RedactBuyerWindow(buyer).Show();
        }

        private void AddBuyerBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddBuyerWindow().Show();
        }

        private void EmlpoyeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee employee = EmlpoyeeDataGrid.SelectedItem as Employee;
            new EditEmlpoyee(employee).Show();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            new AddEmployeeWindow().Show();
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmlpoyeeDataGrid.SelectedItem == null)
                return;

            Employee employee = EmlpoyeeDataGrid.SelectedItem as Employee;

            List<Sypply> sypplies = BDKREntities.GetContext().Sypply
                .Where(s=>s.EmployeeId==employee.EmployeeId)
                .ToList();

            if (sypplies.Count != 0) 
            {
                List<Employee> employees = BDKREntities.GetContext().Employee
                    .Where(emp=>emp.Post.Company.Adress.Contains(employee.Post.Company.Adress))
                    .ToList();

                if (employees.Count != 0)
                {
                    foreach (Sypply sypply in sypplies)
                    {
                        sypply.EmployeeId = employees[0].EmployeeId;
                    }
                }
            }
                BDKREntities.GetContext().Employee.Remove(employee);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Сотрудник удален из бд");
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void UpdateBuyerGrid_Click(object sender, RoutedEventArgs e)
        {
            BDKREntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            BuyerGrid.ItemsSource = BDKREntities.GetContext().Buyer.ToList();
        }

        private void UpdateEmployeeGrid_Click(object sender, RoutedEventArgs e)
        {
            BDKREntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            EmlpoyeeDataGrid.ItemsSource = BDKREntities.GetContext().Employee.ToList();
        }

        private void OrderDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void CreateOrderBt_Click(object sender, RoutedEventArgs e)
        {
            new CreateOrderWindow().Show();
        }

        private void UpdateOrderAndSypplyGrid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateSypplyBt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
