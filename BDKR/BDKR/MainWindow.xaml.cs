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
        }

        private void BuyerGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Buyer buyer = BuyerGrid.SelectedItem as Buyer;
            new RedactBuyerWindow(buyer).Show();

            BDKREntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            BuyerGrid.ItemsSource = BDKREntities.GetContext().Buyer.ToList();
        }

        private void AddBuyerBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddBuyerWindow().Show();
            BDKREntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            BuyerGrid.ItemsSource = BDKREntities.GetContext().Buyer.ToList();
        }

        private void EmlpoyeeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
