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
        public class Prod
        {
            public int ProductId { get; set; }
            public string BrandName { get; set; }

            public string TypeName { get; set; }

            public string ProductName { get; set; }

            public int CountProduct { get; set; }

            public decimal FullPrice { get; set; }
        }

        private Product _selectedProduct;

        private Prod _selectedProductIbnOrder;

        private OrderBuyer order;

        private List<Prod> prod = new List<Prod>();

        public CreateOrderWindow()
        {
            InitializeComponent();
            List<StoregeRoom> storegeRooms = BDKREntities.GetContext().StoregeRoom.ToList();
            List<Product> pr = new List<Product>();
            foreach(StoregeRoom sr in storegeRooms)
            {
                int prId = sr.ProductId;
                var prod = BDKREntities.GetContext().Product.Where(p=>p.ProductId==prId).ToList();
                pr.Add(prod[0]);
            }
            ProductDataGrid.ItemsSource = pr;
            BuyerComboBox.ItemsSource = BDKREntities.GetContext().Buyer.ToList();
            order = new OrderBuyer();
            
        }

        private void AddProductBt_Click(object sender, RoutedEventArgs e)
        {
            if (BuyerComboBox.Text == "")
            {
                MessageBox.Show("Не выбран покупатель для оформления заказа");
                return;
            }
            Prod pr = CheckProductInOrder();
            if (pr != null)
            {
                pr.CountProduct += int.Parse(CountProduct.Text);
                pr.FullPrice = pr.CountProduct * _selectedProduct.PricePerUnit;
                CalculatingFinalPrice();
                BindingSource();
                return;
            }

            decimal fulPrice = ColculatePrice();

            prod.Add(new Prod()
            {
                ProductId = _selectedProduct.ProductId,
                BrandName = _selectedProduct.Brand.BrandName,
                TypeName = _selectedProduct.ProductType.TypeName,
                ProductName = _selectedProduct.ProductName,
                CountProduct = int.Parse(CountProduct.Text),
                FullPrice=fulPrice
            });

            CalculatingFinalPrice();
            BindingSource();
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedProduct = ProductDataGrid.SelectedItem as Product;
            ProductName.Text = _selectedProduct?.ProductName;
            CountProduct.Text = "1";
        }

        private void ProductInOrderDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedProductIbnOrder = ProductInOrderDataGrid.SelectedItem as Prod;
        }

        private void DeleteProductFromOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProductIbnOrder == null)
                return;
            foreach(Prod pr in prod)
            {
                if (pr.ProductId == _selectedProductIbnOrder.ProductId)
                {
                    prod.Remove(pr);
                    CalculatingFinalPrice();
                    BindingSource();
                    return;
                }
            }
        }

        private void ClearOrder_Click(object sender, RoutedEventArgs e)
        {
            prod.Clear();
            CalculatingFinalPrice();
            BindingSource();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (prod.Count == 0)
            {
                MessageBox.Show("Не возможно оформить пустой заказ");
                return;
            }

            order = new OrderBuyer()
            {
                BuyerId = int.Parse(BuyerComboBox.Text),
                OrderCost = decimal.Parse(FullPriceOrder.Text),
                Discount = int.Parse(DiscountOrder.Text),
                CostWitchDiscond = decimal.Parse(FinalyPrice.Text),
                DataOfOrder = DateTime.Now
            };

            BDKREntities.GetContext().OrderBuyer.Add(order);

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }

            List<PositionOrderBuyer> positionOrder = new List<PositionOrderBuyer>();

            foreach (var pr in prod)
            {
                positionOrder.Add(new PositionOrderBuyer()
                {
                    OrderId = order.OrderId,
                    ProductId = pr.ProductId,
                    CountProduct = pr.CountProduct
                });
            }

            foreach(var order in positionOrder)
            {
                BDKREntities.GetContext().PositionOrderBuyer.Add(order);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }

            if (int.Parse(FinalyPrice.Text) > 3000)
            {
                int buyerid = int.Parse(BuyerComboBox.Text);
                var buyer = BDKREntities.GetContext().Buyer
                    .Where(b => b.BuyerId == buyerid)
                    .ToList();

                Random rand = new Random();
                buyer[0].CardNumber = rand.Next(10000, 99999).ToString();

                try
                {
                    BDKREntities.GetContext().SaveChanges();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message.ToString());
                    return;
                }

                CreateSypply();

                MessageBox.Show("Заказ добавлен!");
                this.Close();
            }
        }

        private decimal ColculatePrice()
        {
            decimal price = _selectedProduct.PricePerUnit * decimal.Parse(CountProduct.Text);
            return price;
        }

        private Prod CheckProductInOrder()
        {
            if (prod.Count == 0)
                return null;

            foreach(Prod pr in prod)
            {
                if (pr.ProductId == _selectedProduct.ProductId)
                    return pr;
            }

            return null;
        }

        private void CalculatingFinalPrice()
        {
            if (prod.Count == 0 || prod == null)
            {
                FullPriceOrder.Text = "";
                DiscountOrder.Text = "";
                FinalyPrice.Text = "";
                return;
            }

            decimal fullPriceOrder = 0;

            foreach (Prod pr in prod)
            {
                fullPriceOrder += pr.FullPrice;
            }

            FullPriceOrder.Text = Math.Round(fullPriceOrder, 2).ToString();

            int buyerId = int.Parse(BuyerComboBox.Text);
            var buyer = BDKREntities.GetContext().Buyer
                .Where(b => b.BuyerId == buyerId)
                 .ToList();
            if (buyer[0].CardNumber != null)
            {
                DiscountOrder.Text = "3";
            }
            else
            {
                DiscountOrder.Text = "0";
            }

            if (fullPriceOrder > 10000)
            {
                int disc = int.Parse(DiscountOrder.Text);
                disc += 2;
                DiscountOrder.Text = disc.ToString();
            }

            if (fullPriceOrder > 20000)
            {
                int disc = int.Parse(DiscountOrder.Text);
                disc += 5;
                DiscountOrder.Text = disc.ToString();
            }

            if (DiscountOrder.Text == "0")
            {
                FinalyPrice.Text = fullPriceOrder.ToString();
            }
            else
            {
                decimal finalyPrice = fullPriceOrder - fullPriceOrder*int.Parse(DiscountOrder.Text)/100;
                finalyPrice = Math.Round(finalyPrice, 2);

                FinalyPrice.Text = finalyPrice.ToString();
            }
        }

        private void BindingSource()
        {
            ProductInOrderDataGrid.ItemsSource = null;
            ProductInOrderDataGrid.ItemsSource = prod;
        }

        private void CreateSypply()
        {
            List<Prod> prods = new List<Prod>();
            List<int> cnt = new List<int>();

            foreach(var pr in prod)
            {
                var count = BDKREntities.GetContext().StoregeRoom
                    .Where(st=>st.ProductId==pr.ProductId)
                    .ToList();
                if (count[0].CountProduct < pr.CountProduct)
                {
                    prods.Add(pr);
                    cnt.Add((pr.CountProduct - count[0].CountProduct) * 2);
                }
            }

            if (prods.Count == 0)
                return;

            var empl = BDKREntities.GetContext().Employee.ToList();

            Sypply sypply = new Sypply()
            {
                DateSypply = DateTime.Now,
                EmployeeId = empl[0].EmployeeId
            };

            BDKREntities.GetContext().Sypply.Add(sypply);

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }

            for (int i = 0; i < prods.Count; i++)
            {
                SypplyPosition sPos = new SypplyPosition()
                {
                    SypplyId = sypply.SupplyId,
                    ProductId = prods[i].ProductId,
                    CountProduct = cnt[i],
                };

                BDKREntities.GetContext().SypplyPosition.Add(sPos);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }

            Invoice invoice = new Invoice()
            {
                DdateInvoice = DateTime.Now,
                SypplyId = sypply.SupplyId
            };

            BDKREntities.GetContext().Invoice.Add(invoice);

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }


            for (int i = 0; i < prods.Count; i++)
            {
                InvoicePosition iPos = new InvoicePosition()
                {
                    ProductId = prods[i].ProductId,
                    InvoiceId = invoice.IvoiceId,
                    
                };

                BDKREntities.GetContext().InvoicePosition.Add(iPos);
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }

            for (int i=0; i < prod.Count; i++)
            {
                int prodId = prod[i].ProductId;
                var count = BDKREntities.GetContext().StoregeRoom
                    .Where(sr => sr.ProductId == prodId)
                    .ToList();
                count[0].CountProduct = cnt[i] - prod[i].CountProduct;
            }

            try
            {
                BDKREntities.GetContext().SaveChanges();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
                return;
            }
        }
    }
}
