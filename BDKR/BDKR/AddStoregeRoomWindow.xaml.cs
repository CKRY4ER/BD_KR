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
    /// Логика взаимодействия для AddStoregeRoomWindow.xaml
    /// </summary>
    public partial class AddStoregeRoomWindow : Window
    {
        public AddStoregeRoomWindow()
        {
            InitializeComponent();
            AdressCombo.ItemsSource = BDKREntities.GetContext().Company.ToList();
        }

        private void AddStoreg_Click(object sender, RoutedEventArgs e)
        {
            var company = BDKREntities.GetContext().Company.Where(c => c.Adress.Contains(AdressCombo.Text)).ToList()[0];

            int id = FindeMax();

            StoregeRoom st = new StoregeRoom()
            {
                StorageRoomId = id,
                CompanyId = company.CompanyId,
                ProductId = 7,
                CountProduct = 0
            };

            BDKREntities.GetContext().StoregeRoom.Add(st);

            try
            {
                BDKREntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private int FindeMax()
        {
            var store = BDKREntities.GetContext().StoregeRoom.ToList();
            int id=0;

            foreach(StoregeRoom s in store)
            {
                if (id < s.StorageRoomId)
                    id = s.StorageRoomId;
            }

            id++;
            return id;
        }
    }
}
