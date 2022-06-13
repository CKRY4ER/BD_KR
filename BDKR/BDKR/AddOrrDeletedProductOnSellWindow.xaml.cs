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
    /// Логика взаимодействия для AddOrrDeletedProductOnSellWindow.xaml
    /// </summary>
    public partial class AddOrrDeletedProductOnSellWindow : Window
    {
        private List<Product> prodOnAdd = new List<Product>();

        private Product _selectedOnAdd;

        private Product _selectedOnDelete;

        private class MapperProduct
        {
            public int ProductId { get; set; }

            public string Adress { get; set; } = "";

        }
        public AddOrrDeletedProductOnSellWindow(Product product)
        {
            InitializeComponent();
            Inicialize();

            var storegList = BDKREntities.GetContext().StoregeRoom.ToList();

            List<StoregeRoom> storegeRooms = new List<StoregeRoom>();

            StoregeRoom room = new StoregeRoom() { StorageRoomId = 0 };

            foreach (StoregeRoom storeg in storegList)
            {
                if (storeg.StorageRoomId != room.StorageRoomId)
                {
                    room = storeg;
                    storegeRooms.Add(room);
                }
            }

            StoregRoomBox.ItemsSource = storegeRooms;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (StoregRoomBox.Text == "")
            {
                MessageBox.Show("Не выбран склад!");
                return;
            }

            if (_selectedOnAdd == null)
                return;

            foreach(Product pr in prodOnAdd)
            {
                if (pr.ProductId == _selectedOnAdd.ProductId)
                    return;
            }
            prodOnAdd.Add(_selectedOnAdd);
            Binding();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOnDelete == null)
                return;

            prodOnAdd.Remove(_selectedOnAdd);
            Binding();

        }

        private void AddProductFinal_Click(object sender, RoutedEventArgs e)
        {
            if (prodOnAdd.Count == 0)
                return;

            int id = int.Parse(StoregRoomBox.Text);

            var storeg = BDKREntities.GetContext().StoregeRoom.Where(cp => cp.StorageRoomId == id).ToList()[0];

            var company = BDKREntities.GetContext().Company.Where(sr=>sr.CompanyId == storeg.CompanyId).ToList()[0];

            foreach(Product product in prodOnAdd)
            {
                StoregeRoom storege = new StoregeRoom()
                {
                    StorageRoomId = storeg.StorageRoomId,
                    CompanyId = company.CompanyId,
                    ProductId = product.ProductId,
                    CountProduct = 0
                };

                BDKREntities.GetContext().StoregeRoom.Add(storege);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Товары добавлены в продажу!");
                this.Close();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void ProductNotInSellGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedOnAdd = ProductNotInSellGrid.SelectedItem as Product;
        }

        private void Inicialize()
        {
            List<MapperProduct> mapperProducts = new List<MapperProduct>();

            var product = BDKREntities.GetContext().Product.Where(p=>p.ProductId!=7).ToList();

            var storegRoom = BDKREntities.GetContext().StoregeRoom.ToList();

            foreach (Product prod in product)
            {
                MapperProduct mapProduct = new MapperProduct();

                var content = BDKREntities.GetContext().StoregeRoom.Where(sr => sr.ProductId == prod.ProductId).ToList();

                if (content.Count() != 0)
                {
                    mapProduct.Adress = content[0].Company.Adress;
                }

                mapProduct.ProductId = prod.ProductId;

                mapperProducts.Add(mapProduct);
            }
            mapperProducts = mapperProducts.Where(p => p.Adress == "").ToList();

            if (mapperProducts.Count == 0)
                return;

            List<Product> prodOnBinding = new List<Product>();

            foreach(MapperProduct mapper in mapperProducts)
            {
                var prod = BDKREntities.GetContext().Product.Where(pr => pr.ProductId == mapper.ProductId)
                   .ToList()[0];
                prodOnBinding.Add(prod);
            }

            ProductNotInSellGrid.ItemsSource = prodOnBinding;
        }

        private void Binding()
        {
            ProdOnAddFinal.ItemsSource = null;
            ProdOnAddFinal.ItemsSource = prodOnAdd;
        }

        private void ProdOnAddFinal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedOnDelete = ProdOnAddFinal.SelectedItem as Product;
        }
    }
}
