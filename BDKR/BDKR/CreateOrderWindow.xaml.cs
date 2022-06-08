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
using System.Windows.Shapes;

namespace BDKR
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderWindow.xaml
    /// </summary>
    public partial class CreateOrderWindow : Window
    {
        private Product _selectedProduct;

        private List<PositionOrderBuyer> positionOrder = new List<PositionOrderBuyer>();

        public CreateOrderWindow()
        {
            InitializeComponent();
            ProductDataGrid.ItemsSource = BDKREntities.GetContext().Product.ToList();
        }

        private void AddProductBt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductDataGrid_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ProductInOrderDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DeleteProductFromOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
