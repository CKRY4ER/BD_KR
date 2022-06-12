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
    /// Логика взаимодействия для AddBrandWindow.xaml
    /// </summary>
    public partial class AddBrandWindow : Window
    {
        private Brand brand = new Brand();

        public AddBrandWindow()
        {
            InitializeComponent();
            DataContext = brand;
        }

        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(brand.BrandName))
            {
                MessageBox.Show("Не заполненное поле!");
                return;
            }

            var listBrand = BDKREntities.GetContext().Brand.Where(br => br.BrandName.Contains(brand.BrandName)).ToList();

            if (listBrand.Count != 0)
            {
                MessageBox.Show("Данный бренд уже внесен в БД");
                return;
            }

            BDKREntities.GetContext().Brand.Add(brand);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }
    }
}
