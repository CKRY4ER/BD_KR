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
    /// Логика взаимодействия для FullInfoInvoiceWindow.xaml
    /// </summary>
    public partial class FullInfoInvoiceWindow : Window
    {
        private Invoice _invoice;
        public FullInfoInvoiceWindow(Invoice invoice)
        {
            InitializeComponent();
            _invoice = invoice;
            SypplyDataGrid.ItemsSource = BDKREntities.GetContext().Sypply.Where(s=>s.SupplyId == _invoice.SypplyId).ToList();

            ProductDataGrid.ItemsSource = _invoice.InvoicePosition;
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InvoicePosition invoicePosition = ProductDataGrid.SelectedItem as InvoicePosition;

            Product prod = BDKREntities.GetContext().Product.Where(pr => pr.ProductId == invoicePosition.ProductId).ToList()[0];

            new FullIfoProduct(prod).Show();
        }

        private void SypplyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Sypply sypply = BDKREntities.GetContext().Sypply.Where(s => s.SupplyId == _invoice.SypplyId).ToList()[0];
            new FullInfoSypply(sypply).Show();
        }
    }
}
