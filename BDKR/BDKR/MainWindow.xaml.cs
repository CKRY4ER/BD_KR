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
        private class MapperProduct
        {
            public int ProductId { get; set; }

            public string ArticleNumber { get; set; }

            public string BrandName { get; set; }

            public string TypeName { get; set; }

            public string ProductName { get; set; }

            public string Description { get; set; }

            public int PricePerUnit { get; set; }

            public string Adress { get; set; }

            public string StorageRoomId { get; set; }

            public string CountProduct { get; set; }
        }

        private List<MapperProduct> mapperProducts = new List<MapperProduct>();

        public MainWindow()
        {
            InitializeComponent();
            InicializeProductGrid();
            BuyerGrid.ItemsSource = BDKREntities.GetContext().Buyer.ToList();

            EmlpoyeeDataGrid.ItemsSource = BDKREntities.GetContext().Employee.ToList();

            SypplyDataGrid.ItemsSource = BDKREntities.GetContext().Sypply.ToList();
            OrderDataGrid.ItemsSource = BDKREntities.GetContext().OrderBuyer.ToList();

            ProductDataGrid.ItemsSource = mapperProducts;

            InvoiceDataGrid.ItemsSource = BDKREntities.GetContext().Invoice.ToList();

            CompanyDataGrid.ItemsSource = BDKREntities.GetContext().Company.ToList();

            InicializeStoregRoomGrid();
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

        private void CreateOrderBt_Click(object sender, RoutedEventArgs e)
        {
            new CreateOrderWindow().Show();
        }

        private void UpdateOrderAndSypplyGrid_Click(object sender, RoutedEventArgs e)
        {
            SypplyDataGrid.ItemsSource = null;
            OrderDataGrid.ItemsSource = null;
            SypplyDataGrid.ItemsSource = BDKREntities.GetContext().Sypply.ToList();
            OrderDataGrid.ItemsSource = BDKREntities.GetContext().OrderBuyer.ToList();
        }

        private void CreateSypplyBt_Click(object sender, RoutedEventArgs e)
        {
            new CreateSypply().Show();
        }

        private void OrderDataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            OrderBuyer order = OrderDataGrid.SelectedItem as OrderBuyer;
            new FullInfoOrder(order).Show();
        }

        private void SypplyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Sypply sypply = SypplyDataGrid.SelectedItem as Sypply;
            new FullInfoSypply(sypply).Show();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            new AddProduct(null).Show();
        }

        private void RedactProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem == null)
                return;

            MapperProduct product = ProductDataGrid.SelectedItem as MapperProduct;

            var prod = BDKREntities.GetContext().Product.Where(pr => pr.ProductId == product.ProductId).ToList();

            new AddProduct(prod[0]).Show();
        }

        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            new AddBrandWindow().Show();
        }

        private void RedactBrand_Click(object sender, RoutedEventArgs e)
        {
            new AddAndRedactWindow().Show();
        }

        private void AddType_Click(object sender, RoutedEventArgs e)
        {
            new AddTypeWindow().Show();
        }

        private void RedactType_Click(object sender, RoutedEventArgs e)
        {
            new RedactTypeWindow().Show();
        }

        private void InicializeProductGrid()
        {

            mapperProducts.Clear();
            var product = BDKREntities.GetContext().Product;

            var storegRoom = BDKREntities.GetContext().StoregeRoom.ToList();

            foreach (Product prod in product)
            {
                MapperProduct mapProduct = new MapperProduct();

                var content = BDKREntities.GetContext().StoregeRoom.Where(sr => sr.ProductId == prod.ProductId).ToList();

                if (content.Count() != 0)
                {
                    mapProduct.Adress = content[0].Company.Adress;
                    mapProduct.StorageRoomId = content[0].StorageRoomId.ToString();
                    mapProduct.CountProduct = content[0].CountProduct.ToString();
                }

                mapProduct.ProductId = prod.ProductId;
                mapProduct.ArticleNumber = prod.ArticleNumber;
                mapProduct.BrandName = prod.Brand.BrandName;
                mapProduct.TypeName = prod.ProductType.TypeName;
                mapProduct.ProductName = prod.ProductName;
                mapProduct.Description = prod.Description;
                mapProduct.PricePerUnit = prod.PricePerUnit;

                mapperProducts.Add(mapProduct);
            }
        }

        private void UpdateProductGrid_Click(object sender, RoutedEventArgs e)
        {
            ProductDataGrid.ItemsSource = null;
            InicializeProductGrid();
            ProductDataGrid.ItemsSource = mapperProducts;
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MapperProduct prod = ProductDataGrid.SelectedItem as MapperProduct;

            var product = BDKREntities.GetContext().Product.Where(pr => pr.ProductId == prod.ProductId).ToList();

            new FullIfoProduct(product[0]).Show();
        }

        private void InvoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Invoice invoice = InvoiceDataGrid.SelectedItem as Invoice;
            new FullInfoInvoiceWindow(invoice).Show();
        }

        private void CompanyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Company company = CompanyDataGrid.SelectedItem as Company;

            new FullInfoCompanyWindow(company).Show();
        }

        private void StoregRoomDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCompanyWindow(null).Show();
        }

        private void RedactCompany_Click(object sender, RoutedEventArgs e)
        {
            Company company = CompanyDataGrid.SelectedItem as Company;

            new AddCompanyWindow(company).Show();
        }

        private void DeleteCompany_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateCompanyGrid_Click(object sender, RoutedEventArgs e)
        {
            CompanyDataGrid.ItemsSource = null;
            CompanyDataGrid.ItemsSource = BDKREntities.GetContext().Company.ToList();
        }

        private void InicializeStoregRoomGrid()
        {
            var storegList = BDKREntities.GetContext().StoregeRoom.ToList();

            List<StoregeRoom> storegeRooms = new List<StoregeRoom>();

            StoregeRoom room = new StoregeRoom() { StorageRoomId = 0 };

            foreach(StoregeRoom storeg in storegList)
            {
                if (storeg.CompanyId != room.CompanyId)
                {
                    room = storeg;
                    storegeRooms.Add(room);
                }
            }

            StoregRoomDataGrid.ItemsSource = storegeRooms;
        }
    }
}
