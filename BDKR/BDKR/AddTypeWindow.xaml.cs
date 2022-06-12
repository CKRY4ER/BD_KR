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
    /// Логика взаимодействия для AddTypeWindow.xaml
    /// </summary>
    public partial class AddTypeWindow : Window
    {
        private ProductType _newType = new ProductType();
        public AddTypeWindow()
        {
            InitializeComponent();
            DataContext = _newType;
        }

        private void AddType_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_newType.TypeName))
            {
                MessageBox.Show("Не заполненное поле!");
                return;
            }

            var listType = BDKREntities.GetContext().ProductType.Where(t => t.TypeName.Contains(_newType.TypeName)).ToList();

            if (listType.Count != 0)
            {
                MessageBox.Show("Данный тип уже внесен в БД");
                return;
            }

            BDKREntities.GetContext().ProductType.Add(_newType);

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
