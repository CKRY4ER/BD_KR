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
    /// Логика взаимодействия для DeleteProductFromSell.xaml
    /// </summary>
    public partial class DeleteProductFromSell : Window
    {

        private Product _selectedProduct;
        
        public DeleteProductFromSell()
        {
            InitializeComponent();
            Inicialize();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct == null)
                return;

            var sr = BDKREntities.GetContext().StoregeRoom.Where(s => s.ProductId == _selectedProduct.ProductId).ToList()[0];

            var storeg = BDKREntities.GetContext().StoregeRoom.Where(s => s.StorageRoomId == sr.StorageRoomId).ToList();

            if (storeg.Count <= 1)
            {
                sr.ProductId = 7;
            }
            else
            {
                BDKREntities.GetContext().StoregeRoom.Remove(sr);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Продукт убран из продажи");
                _selectedProduct = null;
                Inicialize();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedProduct = ProductDataGrid.SelectedItem as Product;
        }

        private void Inicialize()
        {
            ProductDataGrid.ItemsSource = null;
            List<StoregeRoom> storegeRooms = BDKREntities.GetContext().StoregeRoom.Where(p => p.ProductId != 7).ToList();
            List<Product> pr = new List<Product>();
            foreach (StoregeRoom sr in storegeRooms)
            {
                int prId = sr.ProductId;
                var prod = BDKREntities.GetContext().Product.Where(p => p.ProductId == prId)
                   .ToList();
                pr.Add(prod[0]);
            }
            ProductDataGrid.ItemsSource = pr;
        }
    }
}
