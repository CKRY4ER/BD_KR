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
    /// Логика взаимодействия для CreateSypply.xaml
    /// </summary>
    public partial class CreateSypply : Window
    {  
        public class Prod
        {
            public int ProductId { get; set; }
            public string BrandName { get; set; }

            public string TypeName { get; set; }

            public string ProductName { get; set; }

            public int CountProduct { get; set; }
        }

        private Employee _employee;

        private Product _selectedProduct;

        private Prod _selectedProdInSypply;

        private List<Prod> prod = new List<Prod>();

        public CreateSypply()
        {
            InitializeComponent();
            EmployeeComboBox.ItemsSource = BDKREntities.GetContext().Employee.ToList();
            List<StoregeRoom> storegeRooms = BDKREntities.GetContext().StoregeRoom.ToList();
            List<Product> pr = new List<Product>();
            foreach (StoregeRoom sr in storegeRooms)
            {
                int prId = sr.ProductId;
                var prod = BDKREntities.GetContext().Product.Where(p => p.ProductId == prId).ToList();
                pr.Add(prod[0]);
            }
            ProductDataGrid.ItemsSource = pr;
        }

        private void DeleteProductFromSypply_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProdInSypply == null)
                return;

            foreach (Prod pr in prod)
            {
                if (pr.ProductId == _selectedProdInSypply.ProductId)
                {
                    prod.Remove(pr);
                    BindingSource();
                    return;
                }
            }
        }

        private void ClearSypply_Click(object sender, RoutedEventArgs e)
        {
            prod.Clear();
            BindingSource();
        }

        private void CreateSypplyBTN_Click(object sender, RoutedEventArgs e)
        {
            int empId = int.Parse(EmployeeComboBox.Text);

            List<SypplyPosition> sypplyPos = new List<SypplyPosition>();

            List<InvoicePosition> invoicePositions = new List<InvoicePosition>();

            Sypply syply = new Sypply()
            {
                EmployeeId = empId,
                DateSypply = DateTime.Now
            };

            BDKREntities.GetContext().Sypply.Add(syply);

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }

            Invoice inv = new Invoice() { SypplyId = syply.SupplyId, DdateInvoice = DateTime.Now };
            BDKREntities.GetContext().Invoice.Add(inv);

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }

            foreach(Prod pr in prod)
            {
                sypplyPos.Add(new SypplyPosition()
                {
                    SypplyId = syply.SupplyId,
                    ProductId = pr.ProductId,
                    CountProduct = pr.CountProduct
                });

                invoicePositions.Add(new InvoicePosition()
                {
                    InvoiceId = inv.IvoiceId,
                    ProductId = pr.ProductId,
                    CountProduct = pr.CountProduct
                });
            }

            foreach(InvoicePosition pos in invoicePositions)
            {
                BDKREntities.GetContext().InvoicePosition.Add(pos);
            }

            foreach(SypplyPosition spos in sypplyPos)
            {
                BDKREntities.GetContext().SypplyPosition.Add(spos);
            }

            foreach(Prod pr in prod)
            {
                var prod = BDKREntities.GetContext().StoregeRoom.Where(p => p.ProductId == pr.ProductId).ToList();
                prod[0].CountProduct += pr.CountProduct;
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Поставка оформлена");
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void AddProductInSypply_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.Text == "")
            {
                MessageBox.Show("Не выбран покупатель для оформления заказа");
                return;
            }

            Prod pr = CheckProductInSypply();

            if (pr != null)
            {
                pr.CountProduct += int.Parse(CountProduct.Text);
                BindingSource();
                return;
            }

            prod.Add(new Prod()
            {
                ProductId = _selectedProduct.ProductId,
                BrandName = _selectedProduct.Brand.BrandName,
                TypeName = _selectedProduct.ProductType.TypeName,
                ProductName = _selectedProduct.ProductName,
                CountProduct = int.Parse(CountProduct.Text),
            });

            BindingSource();
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedProduct = ProductDataGrid.SelectedItem as Product;
            ProductName.Text = _selectedProduct?.ProductName;
            CountProduct.Text = "1";
        }

        private void ProductInSypplyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedProdInSypply = ProductInSypplyDataGrid.SelectedItem as Prod;
        }

        private Prod CheckProductInSypply()
        {
            if (prod.Count == 0)
                return null;

            foreach (Prod pr in prod)
            {
                if (pr.ProductId == _selectedProduct.ProductId)
                    return pr;
            }

            return null;
        }

        private void BindingSource()
        {
            ProductInSypplyDataGrid.ItemsSource = null;
            ProductInSypplyDataGrid.ItemsSource = prod;
        }
    }
}
