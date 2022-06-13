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
    /// Логика взаимодействия для FullInfoStoregRoomWindow.xaml
    /// </summary>
    public partial class FullInfoStoregRoomWindow : Window
    {
        public FullInfoStoregRoomWindow(StoregeRoom storegeRoom)
        {
            InitializeComponent();
            ProductDataGrid.ItemsSource = BDKREntities.GetContext().StoregeRoom.Where(sr => sr.StorageRoomId == storegeRoom.StorageRoomId)
                .Where(pr => pr.ProductId != 7).ToList();
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StoregeRoom sr = ProductDataGrid.SelectedItem as StoregeRoom;

            Product prod = BDKREntities.GetContext().Product.Where(p => p.ProductId == sr.ProductId).ToList()[0];

            new FullIfoProduct(prod).Show();
        }
    }
}
