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
    /// Логика взаимодействия для FullInfoOrder.xaml
    /// </summary>
    public partial class FullInfoOrder : Window
    {
        public class ProductForGrid
        {
            public int Productid { get; set; }

            public string BrandName { get; set; }

            public string TypeName { get; set; }

            public string Name { get; set; }

            public int PricePerUnit { get; set; }

            public int CountProduct { get; set; }

            public int PriceForPosition { get; set; }
        }

        private List<ProductForGrid> prodForGrid = new List<ProductForGrid>();

        public FullInfoOrder(OrderBuyer order)
        {
            InitializeComponent();
            GetProdList(order);
            DataContext = order;
            ProductDataGrid.ItemsSource = prodForGrid;
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProductForGrid prod = ProductDataGrid.SelectedItem as ProductForGrid;

            var product = BDKREntities.GetContext().Product.Where(pr=>pr.ProductId==prod.Productid).ToList();

            new FullIfoProduct(product[0]).Show();
        }

        private void GetProdList(OrderBuyer order)
        {
            var positionOrder = BDKREntities.GetContext().PositionOrderBuyer
                .Where(pos=>pos.OrderId==order.OrderId)
                .ToList();
            
            foreach(PositionOrderBuyer pos in positionOrder)
            {
                var prod = BDKREntities.GetContext().Product
                    .Where(pr=>pr.ProductId==pos.ProductId)
                    .ToList();

                ProductForGrid product = new ProductForGrid()
                {
                    Productid = prod[0].ProductId,
                    BrandName = prod[0].Brand.BrandName,
                    TypeName = prod[0].ProductType.TypeName,
                    Name = prod[0].ProductName,
                    PricePerUnit = prod[0].PricePerUnit,
                    CountProduct = pos.CountProduct
                };
                product.PriceForPosition = product.PricePerUnit * product.CountProduct;

                prodForGrid.Add(product);
            }
        }
    }
}
