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
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private Product _newProduct = new Product();
        public AddProduct(Product product)
        {
            InitializeComponent();

            BrandBox.ItemsSource = BDKREntities.GetContext().Brand.ToList();
            TypeBox.ItemsSource = BDKREntities.GetContext().ProductType.ToList();

            if (product != null)
            {
                DataContext = product;
                BrandBox.Text = product.Brand.BrandName;
                TypeBox.Text = product.ProductType.TypeName;
                AddNewProduct.Click -= AddNewProduct_Click;
                AddNewProduct.Click += Button_Click;
                this.Title = "Редактирование продукта";
                return;
            }

            DataContext = _newProduct;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            Product prod = (Product)DataContext;

            if (string.IsNullOrWhiteSpace(prod.ArticleNumber.ToString()))
                errors.AppendLine("Поле \"Артикул\" является обязательным");
            if (string.IsNullOrWhiteSpace(BrandBox.Text))
                errors.AppendLine("Поле \"Бренд\" является обязательным");
            if (string.IsNullOrWhiteSpace(TypeBox.Text))
                errors.AppendLine("Поле \"Тип\" является обязательным");
            if (string.IsNullOrWhiteSpace(prod.ProductName))
                errors.AppendLine("Поле \"Название продукта\" является обязательным");
            if (string.IsNullOrWhiteSpace(prod.PricePerUnit.ToString()))
                errors.AppendLine("Поле \"Цена за единицу\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            int brandId = BDKREntities.GetContext().Brand.Where(br => br.BrandName.Contains(BrandBox.Text)).ToList()[0].BrandId;

            prod.BrandId = brandId;

            int typeId = BDKREntities.GetContext().ProductType.Where(ty => ty.TypeName.Contains(TypeBox.Text)).ToList()[0].ProductTypeId;

            prod.ProductTypeId = typeId;

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_newProduct.ArticleNumber?.ToString()))
                errors.AppendLine("Поле \"Артикул\" является обязательным");
            if (string.IsNullOrWhiteSpace(BrandBox.Text))
                errors.AppendLine("Поле \"Бренд\" является обязательным");
            if (string.IsNullOrWhiteSpace(TypeBox.Text))
                errors.AppendLine("Поле \"Тип\" является обязательным");
            if (string.IsNullOrWhiteSpace(_newProduct.ProductName))
                errors.AppendLine("Поле \"Название продукта\" является обязательным");
            if (string.IsNullOrWhiteSpace(_newProduct.PricePerUnit.ToString()))
                errors.AppendLine("Поле \"Цена за единицу\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _newProduct.BrandId = BDKREntities.GetContext().Brand.Where(br => br.BrandName.Contains(BrandBox.Text)).ToList()[0].BrandId;
            _newProduct.ProductTypeId = BDKREntities.GetContext().ProductType.Where(ty => ty.TypeName.Contains(TypeBox.Text)).ToList()[0].ProductTypeId;

            BDKREntities.GetContext().Product.Add(_newProduct);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }

        private bool CheckText()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_newProduct.ArticleNumber.ToString()))
                errors.AppendLine("Поле \"Артикул\" является обязательным");
            if (string.IsNullOrWhiteSpace(BrandBox.Text))
                errors.AppendLine("Поле \"Бренд\" является обязательным");
            if (string.IsNullOrWhiteSpace(TypeBox.Text))
                errors.AppendLine("Поле \"Тип\" является обязательным");
            if (string.IsNullOrWhiteSpace(_newProduct.ProductName))
                errors.AppendLine("Поле \"Название продукта\" является обязательным");
            if (string.IsNullOrWhiteSpace(_newProduct.PricePerUnit.ToString()))
                errors.AppendLine("Поле \"Цена за единицу\" является обязательным");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return true;
            }
            return false;
        }
    }
}
