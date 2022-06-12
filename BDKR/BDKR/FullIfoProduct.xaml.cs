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
    /// Логика взаимодействия для FullIfoProduct.xaml
    /// </summary>
    public partial class FullIfoProduct : Window
    {
        public FullIfoProduct(Product product)
        {
            InitializeComponent();
            DataContext = product;
            var storegRoom = BDKREntities.GetContext().StoregeRoom.Where(sr=>sr.ProductId==product.ProductId).ToList();
            if (storegRoom.Count == 0)
            {
                Adress.Text = "";
                Number.Text = "";
                CountProduct.Text = "";
                return;
            }
            int srId = storegRoom[0].StorageRoomId;

            Adress.Text = storegRoom[0].Company.Adress;
            Number.Text = storegRoom[0].StorageRoomId.ToString();
            CountProduct.Text = storegRoom[0].CountProduct.ToString();
        }

        private void Description_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(Description.Text.Trim(' '), "Описание");
        }
    }
}
