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
    /// Логика взаимодействия для RedactTypeWindow.xaml
    /// </summary>
    public partial class RedactTypeWindow : Window
    {
        public RedactTypeWindow()
        {
            InitializeComponent();
            TypeCombo.ItemsSource = BDKREntities.GetContext().ProductType.ToList();
        }

        private void SaveChange_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TypeCombo.Text) || string.IsNullOrWhiteSpace(NameType.Text))
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }

            ProductType type = BDKREntities.GetContext().ProductType.Where(pr => pr.TypeName.Contains(TypeCombo.Text)).ToArray()[0];

            type.TypeName = NameType.Text;

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
