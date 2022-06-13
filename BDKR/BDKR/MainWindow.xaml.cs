using System;
using System.Collections.Generic;
using System.Globalization;
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

        public class DinamicProduct
        {
            public string Month { get; set; }

            public int CoutOrder { get; set; }

            public int CountSypply { get; set; }
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

            ProductArticle.ItemsSource = BDKREntities.GetContext().Product.Where(p => p.ProductId != 7).ToList();

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
            var product = BDKREntities.GetContext().Product.Where(p => p.ProductId != 7).ToList();

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
            if (CompanyDataGrid.SelectedItem == null)
                return;

            Company company = CompanyDataGrid.SelectedItem as Company;

            new FullInfoCompanyWindow(company).Show();
        }

        private void StoregRoomDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StoregeRoom storegeRoom = StoregRoomDataGrid.SelectedItem as StoregeRoom;

            new FullInfoStoregRoomWindow(storegeRoom).Show();
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
            if (CompanyDataGrid.SelectedItem == null)
                return;

            Company company = CompanyDataGrid.SelectedItem as Company;

            var companys = BDKREntities.GetContext().Company.ToList();

            if (companys.Count <=1)
            {
                MessageBox.Show("Не возможно удалить последнюю компанию, тк это нарушит целостность данных!");
                return;
            }

            var sypplys = BDKREntities.GetContext().Sypply
                .Where(s=>s.Employee.Post.Company.CompanyId == company.CompanyId)
                .ToList();

                

            companys = companys.Where(c => c.CompanyId != company.CompanyId).ToList();

            int compId = companys[0].CompanyId;
            Employee empl = BDKREntities.GetContext().Employee
                .Where(em => em.Post.Company.CompanyId == compId)
                .ToList()[0];

            if (sypplys.Count != 0)
            {
                foreach (Sypply syp in sypplys)
                {
                    syp.EmployeeId = empl.EmployeeId;
                }
            }


            var deletedEmployee = BDKREntities.GetContext().Employee.Where(emp => emp.Post.Company.CompanyId == company.CompanyId).ToList();

            if (deletedEmployee.Count != 0)
            {
                foreach (Employee deleteEmployee in deletedEmployee)
                {
                    BDKREntities.GetContext().Employee.Remove(deleteEmployee);
                }
            }

            var deletedPost = BDKREntities.GetContext().Post.Where(p=>p.CompanyId==company.CompanyId).ToList();

            foreach(Post delete in deletedPost)
            {
                BDKREntities.GetContext().Post.Remove(delete);
            }

            BDKREntities.GetContext().Company.Remove(company);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Компания удалена!");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void UpdateCompanyGrid_Click(object sender, RoutedEventArgs e)
        {
            CompanyDataGrid.ItemsSource = null;
            CompanyDataGrid.ItemsSource = BDKREntities.GetContext().Company.ToList();
        }

        private void InicializeStoregRoomGrid()
        {
            StoregRoomDataGrid.ItemsSource = null;

            var storegList = BDKREntities.GetContext().StoregeRoom.ToList();

            List<StoregeRoom> storegeRooms = new List<StoregeRoom>();

            StoregeRoom room = new StoregeRoom() { StorageRoomId = 0 };

            foreach(StoregeRoom storeg in storegList)
            {
                if (storeg.StorageRoomId != room.StorageRoomId)
                {
                    room = storeg;
                    storegeRooms.Add(room);
                }
            }

            StoregRoomDataGrid.ItemsSource = storegeRooms;
        }

        private void AddStoregRom_Click(object sender, RoutedEventArgs e)
        {
            new AddStoregeRoomWindow().Show();
        }

        private void UpdateTableStoreg_Click(object sender, RoutedEventArgs e)
        {
            InicializeStoregRoomGrid();
        }

        private void AddProductOnSell_Click(object sender, RoutedEventArgs e)
        {
            new AddOrrDeletedProductOnSellWindow(null).Show();
        }

        private void DeleteProductFromSell_Click(object sender, RoutedEventArgs e)
        {
            new DeleteProductFromSell().Show();
        }

        private void DeleteStorage_Click(object sender, RoutedEventArgs e)
        {
            StoregeRoom st = StoregRoomDataGrid.SelectedItem as StoregeRoom;

            if (st == null)
                return;

            var store = BDKREntities.GetContext().StoregeRoom.Where(s=>s.StorageRoomId == st.StorageRoomId).ToList();

            foreach (StoregeRoom s in store)
            {
                BDKREntities.GetContext().StoregeRoom.Remove(s);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Склад удален!");
                st = null;
                InicializeStoregRoomGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExecutReporBuyerDate_Click(object sender, RoutedEventArgs e)
        {
            if (DateFirst.Text == "")
            {
                MessageBox.Show("Не выбрана дата!");
                return;
            }
            DateTime date = DateTime.Parse(DateFirst.Text);

            var orders = BDKREntities.GetContext().OrderBuyer
                .Where(o => o.DataOfOrder.Month == date.Month)
                .Where(o => o.DataOfOrder.Year == date.Year)
                .Where(o => o.DataOfOrder.Day == date.Day)
                .Where(o=>o.Discount == 10).ToList();
            
            List<Buyer> buyers = new List<Buyer>();

            foreach(OrderBuyer order in orders)
            {
                Buyer buyer = BDKREntities.GetContext().Buyer.
                    Where(b => b.BuyerId == order.BuyerId).ToList()[0];

                buyers.Add(buyer);
            }

            BuyerOnDate.ItemsSource = buyers;
            CountBuyerOnDate.Text = $"{buyers.Count}";
        }

        private void ProductArticle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductArticle.ItemsSource = null;
            ProductArticle.ItemsSource = BDKREntities.GetContext().Product.Where(p=>p.ProductId!=7).ToList();
        }

        private void Execute2_Click(object sender, RoutedEventArgs e)
        {
            if (ProductArticle.Text == "")
            {
                MessageBox.Show("Не выбран продукт!");
                return;
            }

            Product prod = BDKREntities.GetContext().Product.Where(p=>p.ArticleNumber == ProductArticle.Text).ToList()[0];

            List<DinamicProduct> dinamicProducts = new List<DinamicProduct>();

            DateTime date = new DateTime();

            for (int i = 1; i <= 12; i++)
            {
                var sypplyOnMonth = BDKREntities.GetContext().SypplyPosition.Where(s=>s.Sypply.DateSypply.Month == date.Month)
                    .Where(s=>s.ProductId == prod.ProductId).ToList();

                int countSypply = 0;

                if (sypplyOnMonth.Count != 0)
                {
                    foreach (SypplyPosition sypply in sypplyOnMonth)
                    {
                        countSypply += sypply.CountProduct;
                    }
                }

                var ordersOnMonth = BDKREntities.GetContext().PositionOrderBuyer.Where(p => p.OrderBuyer.DataOfOrder.Month == date.Month)
                    .Where(p => p.ProductId == prod.ProductId).ToList();

                int countOrder = 0;

                if (ordersOnMonth.Count != 0)
                {
                    foreach(PositionOrderBuyer order in ordersOnMonth)
                    {
                        countOrder += order.CountProduct;
                    }
                }

                dinamicProducts.Add(new DinamicProduct()
                {
                    Month = date.ToString("MMMM", CultureInfo.CurrentCulture),
                    CountSypply = countSypply,
                    CoutOrder = countOrder
                });

                date = date.AddMonths(1);
            }

            DinamocProduct.ItemsSource = dinamicProducts;
        }

        private void Execute3_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateNow = DateTime.Now;

            DateTime date = dateNow.AddDays(10);

            var buyer = BDKREntities.GetContext().Buyer
                .ToList();

            List<Buyer> buyers = new List<Buyer>();

            foreach (Buyer bu in buyer)
            {
                if (bu.Birthday.Month == dateNow.Month && bu.Birthday.Day >= dateNow.Day && bu.Birthday.Day <= date.Day)
                {
                    buyers.Add(bu);
                }
            }

            BuyerWithBirthday.ItemsSource = buyers;
        }
    }
}
