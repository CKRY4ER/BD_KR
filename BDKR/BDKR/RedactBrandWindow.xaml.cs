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
    /// Логика взаимодействия для AddAndRedactWindow.xaml
    /// </summary>
    public partial class AddAndRedactWindow : Window
    {
        public AddAndRedactWindow()
        {
            InitializeComponent();
            BrandCombo.ItemsSource = BDKREntities.GetContext().Brand.ToList();
        }

        private void SaveChange_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(BrandCombo.Text) || string.IsNullOrWhiteSpace(NameBrand.Text))
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }

            Brand brand = BDKREntities.GetContext().Brand.Where(br=>br.BrandName.Contains(BrandCombo.Text)).ToList()[0];

            brand.BrandName = NameBrand.Text;

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());
            }
        }
    }
}
